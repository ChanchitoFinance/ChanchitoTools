# Point of View Content Generation Guide

## Overview
This directory contains guidelines and templates for an AI agent that transforms questions into clear, valuable points of view. The agent creates authentic perspectives expressed through logical reasoning and observable patterns, opening with very specific, niche questions.

## Purpose
Transform questions into:
- **Clear points of view** expressed with confidence and reasoning
- **Specific, niche questions** that target precise problems
- **Logical reasoning** backed by observable patterns
- **Actionable insights** that provide genuine value
- **LinkedIn-optimized content** (200 words max)

**CRITICAL**: Never invent or fabricate experiences, stories, or scenarios. Focus on authentic perspectives and observable patterns.

## File Structure

### Core Files
- **STORY_STRUCTURE.md** - The 4-part content framework (Question → Point of View → Insight → Hashtags)
- **CONCISE_STYLE_GUIDE.md** - Techniques for punchy, direct writing
- **WRITING_GUIDELINES.md** - Techniques for expressing clear points of view
- **AI_PROMPT_TEMPLATE.md** - Complete prompt template for point of view content generation
- **PLATFORM_SPECS.md** - Social media platform specs with hashtag strategy (LinkedIn default)

## Quick Start

1. Read `STORY_STRUCTURE.md` to understand the content structure
2. Review `WRITING_GUIDELINES.md` for point of view expression principles
3. Use `AI_PROMPT_TEMPLATE.md` as your generation prompt
4. Focus on specific, niche questions that target precise problems

## Key Principles

- **Concise and punchy**: Short sentences, minimal elaboration
- **Clear point of view**: Express positions with confidence and reasoning
- **Specific, niche questions**: Target precise problems, not generic topics
- **Logical reasoning**: Support perspectives with observable patterns
- **Maximum 200 words**: Excluding hashtags
- **Structure**: Question → Point of View → Insight → Hashtags
- **Always include hashtags**: #code #software #topic #chanchito
- **Platform**: LinkedIn (customizable by developer)
- **NEVER fabricate**: No invented experiences, stories, or scenarios

## Input Format
Very specific, niche questions that target precise problems, such as:
- "Why do teams struggle with domain boundaries when the solution is clearly defined?"
- "What causes developers to add validation in controllers instead of where it actually prevents issues?"
- "Why does 'User' mean three different things in most codebases, and what does that tell us about naming conventions?"

## Output Format
A LinkedIn-ready point of view piece following the prescribed structure, expressing clear perspective with logical reasoning.

**File Format**: All generated content is **appended to a single `Generated_Stories.txt` file**. Each piece includes metadata (question, date, platform, word count) and clear separators. This single-file approach keeps all your content organized, searchable, and easy to manage. Never create separate files—always append to the master file.

**Critical**: Content must express genuine points of view based on observable patterns and principles. Never invent experiences, stories, or scenarios.

