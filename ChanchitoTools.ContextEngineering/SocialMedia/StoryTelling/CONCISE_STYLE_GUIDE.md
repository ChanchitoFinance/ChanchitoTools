# Concise Style Guide

## The Punchy, Direct Approach

Stories should be **concise, punchy, and direct**. Cut unnecessary words. Get to the point. Make every sentence count.

---

## Writing Style Principles

### 1. Short Sentences Win
Break up long sentences into multiple short ones.

❌ **Too Long:**
"My co-founder and I were working on an e-learning platform for a freelancing client and we had a tight deadline of two weeks to get to MVP so we were rushing to get features done."

✅ **Concise:**
"My co-founder and I were building an e-learning platform. Tight deadline. Two weeks to MVP."

### 2. Cut Elaboration
Don't over-explain or add unnecessary context.

❌ **Too Elaborate:**
"We spent several hours going through the git commit history, manually checking out each individual commit one by one, and then testing the application to see if the bug was present in that version or not."

✅ **Direct:**
"Three hours manually checking commits. Testing each one."

### 3. Use Fragments When Effective
Complete sentences aren't always necessary. Fragments add punch.

✅ **Good Use of Fragments:**
- "Tight deadline. Two weeks to MVP."
- "Not important, right? Wrong."
- "The result? Disaster."

### 4. Specific Numbers Over Vague Descriptions
Always prefer concrete numbers to vague descriptions.

❌ **Vague:** "A lot of conflicts"
✅ **Specific:** "347 conflicts"

❌ **Vague:** "Many transactions"
✅ **Specific:** "12,000 transactions"

### 5. Skip Unnecessary Details
Only include details that matter to the lesson.

❌ **Unnecessary Detail:**
"We were sitting in the conference room on the third floor with coffee and whiteboards when the warehouse manager, whose name was Janet, walked in wearing a blue jacket..."

✅ **Essential Only:**
"The warehouse manager frowned at our diagram."

---

## Story Structure in Concise Style

### Opening Hook (20-30 words)
**Goal:** Grab attention immediately. State the insight boldly.

**Style:**
- Short, declarative sentences
- Bold claim or provocative statement
- No warm-up or context

**Examples:**
✅ "Security bugs don't show up in your error logs. They show up in the news."
✅ "Test the flows that make you money first. Everything else can wait."
✅ "Fresh code has fresh bugs. Always."

### Story Body (100-130 words)
**Goal:** Illustrate with minimal elaboration. Get to the point fast.

**Structure:**
1. **Quick Setup** (10-20 words): Context in one sentence
2. **The Problem** (30-50 words): What went wrong, stated clearly
3. **Concrete Examples** (30-50 words): Specific details, numbers
4. **The Lesson** (20-30 words): What you learned

**Style:**
- Short paragraphs (1-3 sentences each)
- Bullet-style thinking in prose form
- Skip transitions and connectors when not needed

**Example:**
```
My co-founder and I built an e-learning platform. We had a bug list.

UI glitch in the video player? Annoying.
Slow page load? Should optimize.
Password reset didn't validate token expiration? We'll get to it.

Week two: "I just reset another user's password using an old link."

We took the site offline. Fixed it immediately. Three users had accounts accessed. Client lost a major customer.
```

### Call to Action (30-50 words)
**Goal:** Give direct, actionable advice. No fluff.

**Style:**
- Command form ("Test X", "Avoid Y", "Start with Z")
- Clear consequences stated
- Strong, definitive language

**Examples:**
✅ "Treat security bugs like production fires. Improper authentication, missing authorization, unvalidated input—these don't just break features, they break trust. Test security paths first."

✅ "Test the flows that make you money first. Everything else can wait. Registration bugs are annoying. Payment bugs are catastrophic."

✅ "Fresh code has fresh bugs. Test what you changed most recently before testing stable code."

### Hashtags (Required)
**Goal:** Increase reach and categorize content

**Format:**
```
#code #software #[topic] #[lesson] #chanchito
```

**Rules:**
- Always start with #code #software
- Always end with #chanchito
- 1-3 topic-specific tags in middle
- Total: 4-6 hashtags
- Place after blank line following story

---

## Editing for Conciseness

### Step 1: Write First Draft
Get the story out. Don't worry about length yet.

### Step 2: Cut 30%
Go through and remove:
- Unnecessary adjectives
- Redundant phrases
- Obvious transitions
- Excessive context

### Step 3: Break Up Sentences
Find sentences over 15 words. Break them into 2-3 shorter ones.

### Step 4: Replace Phrases with Fragments
Turn some sentences into fragments for punch.

### Example Editing Process:

**Draft 1 (250 words):**
"When my co-founder and I were working together on our first major freelancing project, which was a restaurant management system for a local client who needed a point-of-sale integration, we ran into an interesting problem on the very first day when we opened up the codebase..."

**Draft 2 - Cut Unnecessary (180 words):**
"My co-founder and I started our first freelancing project: a restaurant management system. Day one, we opened the codebase..."

**Draft 3 - Break Sentences (170 words):**
"My co-founder and I started our first freelancing project. Restaurant management system. Day one. We opened the codebase..."

**Final - Add Fragments (165 words):**
"First freelancing project. Restaurant management system. Day one, we opened the codebase..."

---

## Concise Language Patterns

### Replace Wordy Phrases

| ❌ Wordy | ✅ Concise |
|---------|-----------|
| "We decided that we should..." | "We decided to..." |
| "It became clear to us that..." | "We realized..." |
| "After several hours of debugging..." | "Three hours debugging..." |
| "In order to fix the issue..." | "To fix it..." |
| "At that point in time..." | "Then..." |
| "We came to the realization that..." | "We learned..." |
| "The thing that we discovered was..." | "We found..." |

### Use Active Voice

| ❌ Passive | ✅ Active |
|-----------|----------|
| "The bug was found by us" | "We found the bug" |
| "Tests were written" | "We wrote tests" |
| "The decision was made" | "We decided" |

### Prefer Strong Verbs

| ❌ Weak | ✅ Strong |
|--------|----------|
| "We had to make a decision" | "We decided" |
| "We took a look at" | "We examined" |
| "We came to understand" | "We learned" |
| "It was necessary to fix" | "We fixed" |

---

## Question-Answer Format (Optional Variation)

For extra punch, use rhetorical questions:

**Pattern:**
```
[Question]? [Short answer].
[Question]? [Short answer].
[Question]? [Impact answer].
```

**Example:**
```
UI glitch in the video player? Annoying.
Slow page load on the dashboard? Should optimize.
Password reset didn't validate the token expiration? We'll get to it.

Week two: "I just reset another user's password."
```

This creates rhythm and engagement.

---

## Hashtag Strategy

### Core Tags (Always Include)
1. **#code** - First tag, broadest reach
2. **#software** - Second tag, tech audience
3. **#chanchito** - Last tag, branding

### Topic Tags (Pick 1-3)
Choose based on story content:

**Development Topics:**
- #testing
- #debugging
- #architecture
- #refactoring
- #codequality
- #cleancode

**Team/Process:**
- #teamwork
- #collaboration
- #conventions
- #standards
- #bestpractices

**Specific Issues:**
- #security
- #performance
- #bugs
- #technical debt
- #legacy code

**Learning/Tips:**
- #tip
- #lesson
- #learntocode
- #devtips
- #programming

**Examples:**
- Security story: `#code #software #security #bugprevention #chanchito`
- Testing story: `#code #software #testing #qualityassurance #chanchito`
- Convention story: `#code #software #conventions #teamwork #chanchito`

### Hashtag Placement
```
[Story ends with call to action]

#code #software #topic #chanchito
```

**DON'T:**
- Mix hashtags into story text
- Use hashtags mid-story
- Add hashtags without spacing

---

## Before/After Examples

### Example 1: Security Bug

**❌ TOO ELABORATE (280 words):**
"When my co-founder and I were building an e-learning platform for one of our freelancing clients, we encountered a situation that taught us a valuable lesson about prioritizing bugs. We had compiled a comprehensive list of various bugs that needed to be addressed before we could launch the platform to real users. Among these bugs, there were several different types..."

**✅ CONCISE (195 words + hashtags):**
```
Security bugs don't show up in your error logs. They show up in the news.

My co-founder and I were building an e-learning platform for a freelancing client. We had a long list of bugs to fix before launch.

UI glitch in the video player? Annoying.
Slow page load on the dashboard? Should optimize.
Password reset didn't validate the token expiration? We'll get to it.

The client pushed to launch. "Those are minor issues."

Week two after launch, we got an email: "I just reset another user's password using an old reset link."

My stomach dropped. Our password reset tokens didn't expire. Someone found an old reset email, used the link, and accessed another account. No exploit needed. Just bad design.

We took the site offline. Fixed it immediately. But the damage was real. Three users had their accounts accessed. The client lost a major customer over the incident.

We'd prioritized visible bugs over invisible vulnerabilities.

Treat security bugs like production fires. Improper authentication, missing authorization, unvalidated input—these don't just break features, they break trust. Test security paths first, even if they're not on your critical user journey. The cost of skipping them is catastrophic.

#code #software #security #tip #chanchito
```

---

## Quality Checklist for Concise Style

Before posting, verify:

- [ ] Story under 200 words (excluding hashtags)
- [ ] Opening hook is 1-2 short sentences
- [ ] Paragraphs are 1-3 sentences each
- [ ] No sentence over 20 words (few exceptions OK)
- [ ] Unnecessary words removed
- [ ] Specific numbers included where relevant
- [ ] Call to action is direct and actionable
- [ ] 4-6 hashtags included
- [ ] #code and #software are first two tags
- [ ] #chanchito is last tag
- [ ] Blank line before hashtags

---

## Summary

**The Golden Rules:**
1. **Cut 30%** - Remove unnecessary words
2. **Short sentences** - Break up long ones
3. **Fragments work** - Use them for punch
4. **Numbers matter** - Be specific
5. **Skip elaboration** - Get to the point
6. **Direct CTA** - Tell them exactly what to do
7. **Always hashtag** - 4-6 tags, format: #code #software #topic #chanchito

**Remember:** Every word should earn its place. If it doesn't add value, cut it.

