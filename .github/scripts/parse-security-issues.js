#!/usr/bin/env node

/**
 * Script để parse security scan results và tạo GitHub Issues
 * Input: JSON file với security findings
 * Output: List of issues to create
 */

const fs = require('fs');
const path = require('path');

const securityFindingsFile = process.argv[2] || 'security-findings.json';
const issuesOutputFile = process.argv[3] || 'security-issues.json';

function determineSeverity(finding) {
    const type = finding.type || '';
    const category = finding.category || '';
    
    // Critical: Hardcoded secrets, SQL injection, XSS
    if (type === 'hardcoded_secret' || type === 'sql_injection' || type === 'xss') {
        return 'critical';
    }
    
    // High: Vulnerable packages, unsafe deserialization, weak crypto
    if (type === 'vulnerable_package' || type === 'unsafe_deserialization' || type === 'weak_crypto') {
        return 'high';
    }
    
    // Medium: Missing validation, insecure config
    if (type === 'missing_validation' || type === 'insecure_config') {
        return 'medium';
    }
    
    // Low: Info leaks, deprecated APIs
    if (type === 'info_leak' || type === 'deprecated_api') {
        return 'low';
    }
    
    return 'medium';
}

function createIssue(finding) {
    const severity = determineSeverity(finding);
    
    const title = `[Security] ${finding.title}`;
    const body = `## Security Finding

**Type:** ${finding.type}
**Category:** ${finding.category}
**Severity:** ${severity.toUpperCase()}

### Description
${finding.description}

### Location
- **File:** \`${finding.file}\`
${finding.line ? `- **Line:** ${finding.line}` : ''}
${finding.code ? `- **Code:** \`\`\`\n${finding.code}\n\`\`\`` : ''}

### Recommendation
${finding.recommendation || 'Please review and fix this security issue.'}

### References
${finding.references ? finding.references.map(ref => `- ${ref}`).join('\n') : '- See security best practices'}`;

    return {
        type: 'security',
        severity,
        title,
        body,
        file: finding.file,
        line: finding.line || null
    };
}

// Main execution
try {
    if (!fs.existsSync(securityFindingsFile)) {
        console.log(`No security findings file found: ${securityFindingsFile}`);
        process.exit(0);
    }
    
    const findings = JSON.parse(fs.readFileSync(securityFindingsFile, 'utf8'));
    const issues = findings.map(finding => createIssue(finding));
    
    fs.writeFileSync(issuesOutputFile, JSON.stringify(issues, null, 2));
    console.log(`✅ Created ${issues.length} security issues in ${issuesOutputFile}`);
    
    // Output summary
    const severityCounts = issues.reduce((acc, issue) => {
        acc[issue.severity] = (acc[issue.severity] || 0) + 1;
        return acc;
    }, {});
    
    console.log('\n📊 Security Issues Summary:');
    Object.entries(severityCounts).forEach(([severity, count]) => {
        console.log(`  ${severity}: ${count}`);
    });
    
} catch (error) {
    console.error('❌ Error parsing security findings:', error.message);
    process.exit(1);
}

