#!/usr/bin/env node

/**
 * Script để parse performance analysis results và tạo GitHub Issues
 * Input: JSON file với performance findings
 * Output: List of issues to create
 */

const fs = require('fs');
const path = require('path');

const performanceFindingsFile = process.argv[2] || 'performance-findings.json';
const issuesOutputFile = process.argv[3] || 'performance-issues.json';

function determineSeverity(finding) {
    const { type, impact, metric } = finding;
    
    // Critical: Memory leaks, N+1 queries, blocking async operations
    if (type === 'memory_leak' || type === 'n_plus_one_query' || type === 'blocking_async') {
        return 'critical';
    }
    
    // High: Large allocations, inefficient algorithms, missing async
    if (type === 'large_allocation' || type === 'inefficient_algorithm' || type === 'missing_async') {
        return 'high';
    }
    
    // Medium: String concatenation, LINQ inefficiencies, unoptimized queries
    if (type === 'string_concat' || type === 'linq_inefficiency' || type === 'unoptimized_query') {
        return 'medium';
    }
    
    // Low/Enhancement: Code complexity, minor optimizations
    if (type === 'complexity' || type === 'minor_optimization') {
        return 'enhancement';
    }
    
    return 'medium';
}

function createIssue(finding) {
    const severity = determineSeverity(finding);
    
    const title = `[Performance] ${finding.title}`;
    const body = `## Performance Finding

**Type:** ${finding.type}
**Impact:** ${finding.impact}
**Severity:** ${severity.toUpperCase()}

### Description
${finding.description}

### Metrics
${finding.metric ? `- **Current:** ${finding.metric.current}\n- **Recommended:** ${finding.metric.recommended}` : 'N/A'}

### Location
- **File:** \`${finding.file}\`
${finding.line ? `- **Line:** ${finding.line}` : ''}
${finding.code ? `- **Code:** \`\`\`csharp\n${finding.code}\n\`\`\`` : ''}

### Recommendation
${finding.recommendation || 'Please review and optimize this code for better performance.'}

### Expected Improvement
${finding.improvement ? finding.improvement : 'Review the code and apply performance best practices.'}`;

    return {
        type: 'performance',
        severity,
        title,
        body,
        file: finding.file,
        line: finding.line || null
    };
}

// Main execution
try {
    if (!fs.existsSync(performanceFindingsFile)) {
        console.log(`No performance findings file found: ${performanceFindingsFile}`);
        process.exit(0);
    }
    
    const findings = JSON.parse(fs.readFileSync(performanceFindingsFile, 'utf8'));
    const issues = findings.map(finding => createIssue(finding));
    
    fs.writeFileSync(issuesOutputFile, JSON.stringify(issues, null, 2));
    console.log(`✅ Created ${issues.length} performance issues in ${issuesOutputFile}`);
    
    // Output summary
    const severityCounts = issues.reduce((acc, issue) => {
        acc[issue.severity] = (acc[issue.severity] || 0) + 1;
        return acc;
    }, {});
    
    console.log('\n📊 Performance Issues Summary:');
    Object.entries(severityCounts).forEach(([severity, count]) => {
        console.log(`  ${severity}: ${count}`);
    });
    
} catch (error) {
    console.error('❌ Error parsing performance findings:', error.message);
    process.exit(1);
}

