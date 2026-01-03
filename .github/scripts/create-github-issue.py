#!/usr/bin/env python3
"""
Script để tạo GitHub Issues tự động từ CI analysis results
"""
import json
import os
import sys
import subprocess
from typing import Dict, List, Optional

def check_existing_issue(title: str, repo: str, label: str) -> Optional[int]:
    """Kiểm tra xem issue đã tồn tại chưa"""
    try:
        result = subprocess.run(
            ['gh', 'issue', 'list', '--repo', repo, '--label', label, '--state', 'open', 
             '--search', f'in:title "{title}"', '--json', 'number', '--jq', '.[0].number'],
            capture_output=True,
            text=True,
            timeout=10
        )
        if result.returncode == 0 and result.stdout.strip():
            issue_num = result.stdout.strip()
            if issue_num != 'null' and issue_num.isdigit():
                return int(issue_num)
    except Exception:
        pass
    return None

def create_issue(issue_data: Dict, repo: str, github_token: str) -> Optional[int]:
    """Tạo GitHub Issue"""
    title = issue_data.get('title', 'Untitled Issue')
    body = issue_data.get('body', '')
    severity = issue_data.get('severity', 'medium')
    issue_type = issue_data.get('type', 'general')
    file_path = issue_data.get('file', '')
    line_number = issue_data.get('line')
    
    # Xây dựng labels
    labels = f"severity:{severity},type:{issue_type}"
    
    # Kiểm tra issue đã tồn tại
    existing = check_existing_issue(title, repo, f"severity:{severity}")
    if existing:
        print(f"⚠️ Issue already exists: #{existing}")
        return existing
    
    # Thêm thông tin chi tiết vào body
    enhanced_body = f"""{body}

---
**Severity:** {severity.upper()}
**Type:** {issue_type}
**Branch:** {os.getenv('GITHUB_REF', '').replace('refs/heads/', '')}
**File:** {file_path}{f' (Line {line_number})' if line_number else ''}
**Detected by:** CI Pipeline
**Workflow Run:** {os.getenv('GITHUB_SERVER_URL', '')}/{repo}/actions/runs/{os.getenv('GITHUB_RUN_ID', '')}

---
*This issue was automatically created by the CI pipeline.*"""
    
    try:
        # Tạo issue bằng GitHub CLI
        env = os.environ.copy()
        env['GH_TOKEN'] = github_token
        
        result = subprocess.run(
            ['gh', 'issue', 'create',
             '--repo', repo,
             '--title', title,
             '--body', enhanced_body,
             '--label', labels],
            capture_output=True,
            text=True,
            env=env,
            timeout=30
        )
        
        if result.returncode == 0:
            # Extract issue number from output
            output = result.stdout.strip()
            if '#' in output:
                issue_num = output.split('#')[1].split()[0]
                if issue_num.isdigit():
                    print(f"✅ Created issue #{issue_num}: {title} ({severity})")
                    return int(issue_num)
        else:
            print(f"❌ Failed to create issue: {result.stderr}", file=sys.stderr)
    except Exception as e:
        print(f"❌ Error creating issue: {e}", file=sys.stderr)
    
    return None

def main():
    if len(sys.argv) < 3:
        print("Usage: create-github-issue.py <issues-json-file> <repo> [github-token]")
        sys.exit(1)
    
    issues_file = sys.argv[1]
    repo = sys.argv[2]
    github_token = sys.argv[3] if len(sys.argv) > 3 else os.getenv('GITHUB_TOKEN', '')
    
    if not github_token:
        print("⚠️ GITHUB_TOKEN not provided, skipping issue creation")
        sys.exit(0)
    
    if not os.path.exists(issues_file):
        print(f"⚠️ Issues file not found: {issues_file}")
        sys.exit(0)
    
    try:
        with open(issues_file, 'r', encoding='utf-8') as f:
            issues = json.load(f)
        
        if not isinstance(issues, list):
            issues = [issues]
        
        created_count = 0
        for issue_data in issues:
            issue_num = create_issue(issue_data, repo, github_token)
            if issue_num:
                created_count += 1
        
        print(f"\n📊 Created {created_count} out of {len(issues)} issues")
        
    except json.JSONDecodeError as e:
        print(f"❌ Error parsing JSON: {e}", file=sys.stderr)
        sys.exit(1)
    except Exception as e:
        print(f"❌ Error: {e}", file=sys.stderr)
        sys.exit(1)

if __name__ == '__main__':
    main()

