#!/usr/bin/env python3
"""Script để export secret findings từ grep results sang JSON"""
import json
import sys
import os

def export_secret_findings(secrets_array, output_file):
    """Export secret findings array to JSON file"""
    findings = []
    
    # Parse secrets from array format
    for secret_info in secrets_array:
        if not secret_info or ':' not in secret_info:
            continue
        
        parts = secret_info.split(':', 2)
        if len(parts) >= 2:
            file_path = parts[0]
            line_num = parts[1]
            code = parts[2] if len(parts) > 2 else ""
            
            # Skip comments and test files
            if code.strip().startswith('//') or 'test' in file_path.lower() or 'spec' in file_path.lower():
                continue
            
            findings.append({
                "type": "hardcoded_secret",
                "category": "security",
                "title": "Hardcoded secret detected",
                "file": file_path,
                "line": line_num,
                "description": "Potential hardcoded secret found in code",
                "code": code.replace('"', '\\"').replace('\n', ' '),
                "recommendation": "Move secrets to configuration files or use Azure Key Vault"
            })
    
    with open(output_file, 'w', encoding='utf-8') as f:
        json.dump(findings, f, indent=2, ensure_ascii=False)
    
    return len(findings)

if __name__ == '__main__':
    # Read from environment or command line
    secrets_str = os.getenv('SECRETS_FOUND', '')
    if not secrets_str and len(sys.argv) > 1:
        secrets_str = sys.argv[1]
    
    output_file = sys.argv[2] if len(sys.argv) > 2 else 'secret-findings.json'
    
    # Parse secrets (space-separated or newline-separated)
    secrets_array = [s.strip() for s in secrets_str.replace('\n', ' ').split() if s.strip()]
    
    count = export_secret_findings(secrets_array, output_file)
    print(f"Exported {count} secret findings to {output_file}")

