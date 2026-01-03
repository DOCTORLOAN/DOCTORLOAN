#!/usr/bin/env python3
"""Script để analyze code complexity và export findings"""
import os
import json
import subprocess
from pathlib import Path

def find_long_files(project_dir, max_lines=500):
    """Tìm các file có số dòng vượt quá max_lines"""
    findings = []
    project_path = Path(project_dir)
    
    for cs_file in project_path.rglob("*.cs"):
        # Skip obj and bin directories
        if "obj" in cs_file.parts or "bin" in cs_file.parts:
            continue
        
        try:
            with open(cs_file, 'r', encoding='utf-8', errors='ignore') as f:
                lines = sum(1 for _ in f)
            
            if lines > max_lines:
                relative_path = str(cs_file.relative_to(project_path.parent))
                findings.append({
                    "type": "complexity",
                    "title": f"Large file detected: {cs_file.name}",
                    "file": relative_path,
                    "description": f"File has {lines} lines (>500 recommended)",
                    "recommendation": "Consider refactoring into smaller files/classes",
                    "improvement": "Reducing file size improves maintainability and testability"
                })
        except Exception:
            pass
    
    return findings

def find_performance_patterns(project_dir):
    """Tìm các performance anti-patterns"""
    findings = []
    project_path = Path(project_dir)
    
    blocking_patterns = [
        (r"\.Result\b", "blocking_async", "Blocking async operation (.Result)"),
        (r"\.Wait\(\)", "blocking_async", "Blocking async operation (.Wait())"),
        (r"\.GetAwaiter\(\)\.GetResult\(\)", "blocking_async", "Blocking async operation (.GetAwaiter().GetResult())"),
    ]
    
    found_patterns = {}
    
    for cs_file in project_path.rglob("*.cs"):
        if "obj" in cs_file.parts or "bin" in cs_file.parts:
            continue
        
        try:
            with open(cs_file, 'r', encoding='utf-8', errors='ignore') as f:
                content = f.read()
                lines = content.split('\n')
                
                for line_num, line in enumerate(lines, 1):
                    # Skip comments
                    if line.strip().startswith('//'):
                        continue
                    
                    for pattern, pattern_type, description in blocking_patterns:
                        import re
                        if re.search(pattern, line):
                            key = f"{pattern_type}:{cs_file.name}"
                            if key not in found_patterns:
                                found_patterns[key] = {
                                    "type": pattern_type,
                                    "title": description,
                                    "file": str(cs_file.relative_to(project_path.parent)),
                                    "description": f"Found {description.lower()}",
                                    "recommendation": "Use async/await properly instead of blocking calls",
                                    "impact": "high"
                                }
        except Exception:
            pass
    
    return list(found_patterns.values())

if __name__ == '__main__':
    import sys
    
    project_dir = sys.argv[1] if len(sys.argv) > 1 else 'DOCTORLOAN'
    output_file = sys.argv[2] if len(sys.argv) > 2 else 'complexity-findings.json'
    
    complexity_findings = find_long_files(project_dir)
    performance_findings = find_performance_patterns(project_dir)
    
    all_findings = complexity_findings + performance_findings
    
    with open(output_file, 'w', encoding='utf-8') as f:
        json.dump(all_findings, f, indent=2, ensure_ascii=False)
    
    print(f"Found {len(all_findings)} issues:")
    print(f"  - Complexity: {len(complexity_findings)}")
    print(f"  - Performance patterns: {len(performance_findings)}")

