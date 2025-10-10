# Story Structure Guide

## The 3-Part Framework

Every story MUST follow this exact structure:

### Part 1: Main Value (Opening Hook)
**Length**: 1-2 sentences (20-30 words)
**Purpose**: Immediately state the core insight or value
**Tone**: Bold, clear, provocative

**Characteristics:**
- Starts with a powerful statement or question
- Promises value to the reader
- Creates curiosity
- NO fluff or setup

**Examples:**
- "The best domain models don't start in code. They start in conversations."
- "Overengineering kills more projects than underengineering ever could."
- "Time isn't just a timestamp. It's the story of how things change."

---

### Part 2: The Story (Body)
**Length**: 100-130 words
**Purpose**: Show a real-life scene with minimal elaboration
**Tone**: Direct, punchy, conversational

**MUST Include:**
- **Quick scene setup** - no lengthy descriptions
- **Concrete examples** - specific numbers, actions
- **The problem** stated clearly
- **The realization** or turning point
- **Short, punchy sentences** for impact

**MUST AVOID:**
- Long explanations or backstory
- Overly detailed descriptions
- Multiple scenes in one story
- Lengthy dialogue
- Verbose language

**Concise vs Verbose:**
- ❌ VERBOSE: "We spent three hours in a conference room with whiteboards full of diagrams trying to understand what the warehouse manager was telling us about how the order processing system worked in their facility."
- ✅ CONCISE: "The warehouse manager frowned at our 'Order' class. 'We don't process orders. We pick, pack, and ship. Three different systems.'"

---

### Part 3: Call to Action or Valuable Tip (Closing)
**Length**: 30-50 words
**Purpose**: Drive home the lesson with actionable advice
**Tone**: Direct, authoritative, clear

**Options:**
1. **Strong directive**: Tell them exactly what to do
2. **Key principle**: Memorable rule to follow
3. **Warning**: What NOT to do and why

**Examples:**
- "Test security paths first. Improper authentication, missing authorization, unvalidated input—these don't just break features, they break trust."
- "Test the flows that make you money first. Everything else can wait."
- "Fresh code has fresh bugs. Test what you changed most recently."

### Part 4: Hashtags (Required)
**Length**: 4-6 hashtags
**Purpose**: Increase reach and discoverability
**Placement**: After the story, separated by a blank line

**Format:**
```
[Story content ends]

#code #software #[topic] #[lesson1] #[lesson2] #chanchito
```

**Examples:**
- `#code #software #security #tip #chanchito`
- `#code #software #testing #bugprevention #chanchito`
- `#code #software #conventions #teamwork #chanchito`

**Always include:**
- `#code` (first)
- `#software` (second)
- `#chanchito` (last)
- 1-3 topic-specific hashtags in between

---

## Complete Story Formula

```
[MAIN VALUE: 1-2 punchy sentences stating the core insight]

[STORY: 100-130 words with:
 - Quick scene setup
 - Concrete examples (numbers, specifics)
 - Problem clearly stated
 - Realization or lesson
 - Short, punchy sentences]

[CALL TO ACTION: 30-50 words giving:
 - Direct actionable advice, OR
 - Strong principle, OR
 - Clear warning]

[HASHTAGS: 4-6 tags]
#code #software #[topic] #[lesson] #chanchito
```

---

## Hard Rules

✅ **DO:**
- Keep total word count under 200 words (story + CTA, excluding hashtags)
- Use short paragraphs (1-3 sentences)
- Use short, punchy sentences
- Include specific numbers and examples
- Focus on one single insight
- End with clear, direct advice
- Always include 4-6 hashtags

❌ **DON'T:**
- Exceed 200 words for story content
- Over-elaborate or add unnecessary details
- Use multiple examples in one story
- Include long dialogue exchanges
- Write lengthy scene descriptions
- Forget hashtags
- Use more than 6 hashtags

---

## Format Specifications

### Paragraph Breaks
- Main Value: Standalone paragraph (1-2 sentences)
- Story: 2-4 SHORT paragraphs with line breaks between
- Call to Action: Standalone paragraph (1-2 sentences)
- Hashtags: Separate line after blank line

### Line Spacing
- Single blank line between paragraphs within story
- Double blank line before hashtags

### Emphasis (Optional)
- Use **bold** sparingly for key phrases
- Never use ALL CAPS (except for acronyms)
- Emoji use: Maximum 0-1, only if natural

### Hashtag Formatting
```
[Last sentence of story]

#code #software #topic #chanchito
```

---

## Quality Checklist

Before finalizing a story, verify:

- [ ] Total word count ≤ 200 words
- [ ] Opens with clear value statement
- [ ] Contains a specific scene (not abstract explanation)
- [ ] Includes emotional element
- [ ] Shows character action or realization
- [ ] Ends with actionable takeaway
- [ ] Uses "show, don't tell" technique
- [ ] Feels personal and relatable
- [ ] LinkedIn-appropriate tone (professional but human)
- [ ] Appended to `Generated_Stories.txt` with metadata and separators

