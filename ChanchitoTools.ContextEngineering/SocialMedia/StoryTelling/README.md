# StoryTelling AI Agent Guide

## Overview
This directory contains guidelines and templates for an AI agent that transforms technical questions into engaging LinkedIn stories. The agent creates short, emotionally-resonant narratives that connect abstract concepts to real-world experiences.

## Purpose
Transform technical or conceptual questions into:
- **Engaging stories** that show rather than tell
- **Emotional connections** with readers' experiences
- **Actionable insights** delivered through narrative
- **LinkedIn-optimized content** (200 words max)

## File Structure

### Core Files
- **STORY_STRUCTURE.md** - The 4-part story framework (Value → Story → CTA → Hashtags)
- **CONCISE_STYLE_GUIDE.md** - Techniques for punchy, direct writing
- **WRITING_GUIDELINES.md** - Techniques for emotional connection and "showing"
- **EXAMPLES.md** - Reference stories demonstrating best practices
- **AI_PROMPT_TEMPLATE.md** - Complete prompt template for story generation
- **PLATFORM_SPECS.md** - Social media platform specs with hashtag strategy (LinkedIn default)

## Quick Start

1. Read `STORY_STRUCTURE.md` to understand the 3-part format
2. Review `WRITING_GUIDELINES.md` for writing principles
3. Study `EXAMPLES.md` to see the format in action
4. Use `AI_PROMPT_TEMPLATE.md` as your generation prompt

## Key Principles

- **Concise and punchy**: Short sentences, minimal elaboration
- **Show, don't tell**: Quick scenes with concrete examples
- **Specific numbers**: 347 conflicts, 12,000 transactions, 3 weeks
- **Direct advice**: Tell readers exactly what to do
- **Maximum 200 words**: Excluding hashtags
- **Structure**: Value → Story → CTA → Hashtags
- **Always include hashtags**: #code #software #topic #chanchito
- **Platform**: LinkedIn (customizable by developer)

## Input Format
Any technical or conceptual question, such as:
- "What's the first step to finding real-world DDD concepts?"
- "How do we avoid overcomplicating early domain models?"
- "How do we model values that change over time (like score)?"

## Output Format
A LinkedIn-ready story following the prescribed structure, emotionally engaging, and actionable.

**File Format**: All generated stories are **appended to a single `Generated_Stories.txt` file**. Each story includes metadata (question, date, platform, word count) and clear separators. This single-file approach keeps all your content organized, searchable, and easy to manage. Never create separate files—always append to the master file.

