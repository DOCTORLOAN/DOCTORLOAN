#!/bin/bash

# Script để tạo GitHub Issues tự động từ CI analysis results
# Usage: ./create-issues.sh <issue-type> <severity> <title> <body> <file-path> [line-number]

set -e

ISSUE_TYPE=$1
SEVERITY=$2
TITLE=$3
BODY=$4
FILE_PATH=$5
LINE_NUMBER=${6:-""}

if [ -z "$ISSUE_TYPE" ] || [ -z "$SEVERITY" ] || [ -z "$TITLE" ] || [ -z "$BODY" ]; then
    echo "Usage: $0 <issue-type> <severity> <title> <body> <file-path> [line-number]"
    exit 1
fi

# GitHub repository info
REPO="${GITHUB_REPOSITORY}"
BRANCH="${GITHUB_REF#refs/heads/}"

# Map severity to GitHub label color
case "$SEVERITY" in
    "critical")
        LABEL="severity:critical"
        COLOR="d73a4a"  # Red
        ;;
    "high")
        LABEL="severity:high"
        COLOR="e99695"  # Light red
        ;;
    "medium")
        LABEL="severity:medium"
        COLOR="fbca04"  # Yellow
        ;;
    "low")
        LABEL="severity:low"
        COLOR="0e8a16"  # Green
        ;;
    "enhancement")
        LABEL="severity:enhancement"
        COLOR="1d76db"  # Blue
        ;;
    *)
        LABEL="severity:medium"
        COLOR="fbca04"
        ;;
esac

# Add type label
TYPE_LABEL="type:${ISSUE_TYPE}"
LABELS="${LABEL},${TYPE_LABEL}"

# Build issue body with details
ISSUE_BODY="${BODY}

---

**Severity:** ${SEVERity^^}
**Type:** ${ISSUE_TYPE}
**Branch:** ${BRANCH}
**File:** ${FILE_PATH}${LINE_NUMBER:+ (Line ${LINE_NUMBER})}
**Detected by:** CI Pipeline
**Workflow Run:** ${GITHUB_SERVER_URL}/${GITHUB_REPOSITORY}/actions/runs/${GITHUB_RUN_ID}

---
*This issue was automatically created by the CI pipeline.*"

# Check if issue already exists (prevent duplicates)
EXISTING_ISSUE=$(gh issue list \
    --repo "$REPO" \
    --label "$LABEL" \
    --state open \
    --search "in:title \"${TITLE}\"" \
    --json number --jq '.[0].number' 2>/dev/null || echo "")

if [ -n "$EXISTING_ISSUE" ] && [ "$EXISTING_ISSUE" != "null" ]; then
    echo "⚠️ Issue already exists: #${EXISTING_ISSUE}"
    exit 0
fi

# Create issue
ISSUE_NUMBER=$(gh issue create \
    --repo "$REPO" \
    --title "$TITLE" \
    --body "$ISSUE_BODY" \
    --label "$LABELS" \
    --json number --jq '.number' 2>/dev/null || echo "")

if [ -n "$ISSUE_NUMBER" ] && [ "$ISSUE_NUMBER" != "null" ]; then
    echo "✅ Created issue #${ISSUE_NUMBER}: ${TITLE} (${SEVERITY})"
    echo "${ISSUE_NUMBER}" >> "${GITHUB_STEP_SUMMARY}"
else
    echo "❌ Failed to create issue: ${TITLE}"
    exit 1
fi

