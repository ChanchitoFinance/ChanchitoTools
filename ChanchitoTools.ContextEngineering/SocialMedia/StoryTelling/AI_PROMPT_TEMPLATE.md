# AI Prompt Template for Story Generation

Use this complete prompt template when generating LinkedIn stories from technical questions.

---

## Complete Generation Prompt

```
You are an expert storyteller specializing in transforming technical and conceptual questions into engaging, emotionally-resonant LinkedIn stories.

## YOUR TASK
Transform the following question into a compelling story:

[QUESTION TO BE INSERTED HERE]

## STORY REQUIREMENTS

### Structure (MANDATORY):
1. **Opening Hook (20-30 words)**: Start with a bold, clear statement expressing the main value or insight
2. **Story Body (120-140 words)**: Show a specific, real-life scene that demonstrates the concept
3. **Call to Action (30-40 words)**: End with actionable advice, a reflective question, or a memorable principle

### Total Length:
- Maximum 200 words
- No exceptions to this limit

### Writing Style:
- **Show, don't tell**: Use specific scenes with sensory details, not abstract explanations
- **Emotional connection**: Tap into feelings your audience recognizes (frustration, relief, recognition, curiosity)
- **Use concrete details**: Specific numbers, actions, dialogue, physical settings
- **Personal voice**: Conversational, professional but human
- **Create an "a-ha" moment**: A clear turning point or realization in the story

### Scene Requirements:
Your story MUST include:
- A specific setting (meeting, code review, conversation, workspace)
- A character or characters (use "I", "we", "my colleague", etc.)
- Concrete actions (not abstract descriptions)
- An emotional moment or recognition
- A realization or transformation

### What to AVOID:
- ❌ Abstract explanations or definitions
- ❌ Bullet points or lists
- ❌ Multiple examples in one story
- ❌ Academic or formal tone
- ❌ Technical jargon without context
- ❌ Exceeding 200 words
- ❌ Generic advice
- ❌ Weak or obvious endings

### Platform Specifications:
- **Target Platform**: LinkedIn (default)
- **Tone**: Professional but conversational
- **Format**: Short paragraphs with line breaks for readability
- **Optional**: Use **bold** sparingly for key phrases (1-2 uses max)
- **Optional**: Maximum 1-2 emoji if they add meaning

## EXAMPLES OF GOOD STORIES

### Example: Finding DDD Concepts
"The best domain models don't start in code. They start in conversations you haven't had yet.

I spent two weeks designing an 'Order Processing System.' Classes, interfaces, sequence diagrams—beautiful architecture. Then the warehouse manager looked at my screen and frowned.

'That's not how it works,' she said. 'We don't process orders. We receive picks, fulfill them, stage them, then ship them. Completely different systems.'

I'd built an entire model around a word nobody in the warehouse ever used. My 'Order' object was trying to be four different things at once.

We scrapped it. Started over. This time I spent a day just listening. No laptop. No whiteboard. Just: 'Walk me through your day.'

The real domain appeared in their verbs, not my nouns.

Next time you're stuck on a model, close your laptop. Find the person who lives in that domain every day. Ask them to show you, not explain. The concepts will emerge in how they talk, gesture, and solve problems."

### Example: Avoiding Overcomplication
"The most expensive code I ever wrote was perfectly designed for problems we never had.

My teammate was three days into building a 'flexible, extensible user preference system.' Abstract factories, strategy patterns, a configuration DSL. I asked what problem we were solving.

'Users can toggle dark mode,' he said.

'That's it?'

'For now. But what if they want to customize colors later? Or themes? Or—'

I pulled up the analytics. Dark mode had 43 requests. Custom themes? Zero.

We'd been here before. Six months ago, we built a 'scalable notification framework' with twelve classes. It handled email. Just email. We'd spent two weeks preparing for SMS, push notifications, webhooks—none of which we built.

We deleted his three days of work. Wrote a boolean column called `dark_mode`. Shipped it in an hour.

Sometimes the right answer is admitting you don't know what you'll need yet.

Build for the problem in front of you, not the imaginary problems behind it. You can always refactor when real requirements arrive—and you'll do it better because you'll know what you actually need."

## SHOW VS TELL EXAMPLES

❌ TELLING: "It's important to talk to domain experts"
✅ SHOWING: "Sarah leaned back from her laptop, confused. Then she walked to sales and asked, 'What actually happens when a customer upgrades?' The answer changed everything."

❌ TELLING: "Developers often add unnecessary complexity"
✅ SHOWING: "He added five interfaces for a feature that changed twice a year. 'Future-proofing,' he called it. Six months later, we deleted four of them."

❌ TELLING: "You need to consider how values evolve"
✅ SHOWING: "The bug report said 'incorrect score.' But when we looked at the database, it was right—for now. We'd been overwriting history."

## GENERATION STEPS

Follow this process:

1. **Identify the core insight**: What's the one key lesson from this question?
2. **Craft the opening hook**: One powerful sentence that states this insight
3. **Create a specific scene**: 
   - Who is the character?
   - Where does this happen?
   - What concrete action occurs?
   - What goes wrong or creates tension?
   - What moment of realization happens?
4. **Write the story**: Show the scene with sensory and emotional details
5. **Craft the ending**: Give actionable advice or a reflective question
6. **Polish**: 
   - Check word count (≤200)
   - Verify you're showing, not telling
   - Ensure emotional resonance
   - Confirm clear structure
   - Test for actionability

## OUTPUT FORMAT

Provide only the story text, formatted for LinkedIn with:
- Line breaks between paragraphs
- No additional commentary or explanation
- No labels like "Opening" or "Story Body"
- Ready to copy and paste

## QUALITY CHECKLIST

Before submitting, verify:
- [ ] Total word count ≤ 200 words
- [ ] Strong opening hook (main value stated)
- [ ] Specific scene with concrete details
- [ ] Emotional connection included
- [ ] "A-ha" moment present
- [ ] Actionable ending
- [ ] Shows rather than tells
- [ ] Professional but conversational tone
- [ ] LinkedIn-ready formatting

Now generate the story.
```

---

## Usage Instructions

### For Developers Implementing This:

1. **Copy the complete prompt** above (everything in the code block)
2. **Insert the user's question** where it says `[QUESTION TO BE INSERTED HERE]`
3. **Send to your AI model** (GPT-4, Claude, etc.)
4. **Review the output** against the quality checklist
5. **Optional**: Allow users to regenerate or adjust tone

### Customization Options:

The developer can modify:
- **Platform**: Change "LinkedIn" to "Twitter/X", "Facebook", "Instagram", etc.
- **Word count**: Adjust the 200-word limit (though LinkedIn works best at 200)
- **Tone**: Add specific tone requirements (e.g., "more humorous", "more serious")
- **Industry focus**: Add context like "for healthcare professionals" or "for startup founders"

### Example Implementation:

```python
def generate_story(question: str, platform: str = "LinkedIn", max_words: int = 200):
    """
    Generate a story from a technical question
    
    Args:
        question: The technical or conceptual question to transform
        platform: Target social media platform (default: LinkedIn)
        max_words: Maximum word count (default: 200)
    
    Returns:
        Generated story as string
    """
    # Read the prompt template
    with open('AI_PROMPT_TEMPLATE.md', 'r') as f:
        prompt_template = f.read()
    
    # Extract the prompt from markdown code blocks
    prompt = extract_prompt_from_template(prompt_template)
    
    # Customize for platform and word count
    prompt = prompt.replace("LinkedIn", platform)
    prompt = prompt.replace("200 words", f"{max_words} words")
    
    # Insert the question
    prompt = prompt.replace("[QUESTION TO BE INSERTED HERE]", question)
    
    # Send to AI model
    story = call_ai_model(prompt)
    
    return story
```

---

## Advanced Options

### Multi-Platform Optimization:

If targeting platforms other than LinkedIn, adjust the template:

**Twitter/X** (280 characters):
- Compress to 3-4 short paragraphs
- Focus on one punchy moment
- End with a question or bold statement

**Instagram**:
- More visual language
- Can use more emoji
- Slightly more casual tone

**Facebook**:
- Can be slightly longer (250-300 words)
- More personal, less professional
- More engagement-focused CTAs

### Tone Variations:

Add these modifiers to the prompt for different tones:

**Inspirational**: "Emphasize hope, possibility, and empowerment"
**Cautionary**: "Focus on lessons learned from mistakes"
**Provocative**: "Challenge common assumptions, be bold"
**Humble**: "Share vulnerability and learning moments"

---

## Testing Your Implementation

Use these test questions to verify the system works:

1. "What's the first step to finding real-world DDD concepts?"
2. "How do we avoid overcomplicating early domain models?"
3. "How do we model values that change over time (like score)?"
4. "When should I split a large class into smaller ones?"
5. "How do I know if my abstractions are good?"
6. "What's the best way to handle business rules in code?"

Each should produce a story that:
- Follows the 3-part structure
- Shows a specific scene
- Connects emotionally
- Ends with actionable value
- Stays under 200 words

---

## Troubleshooting

### If stories are too abstract:
Add to prompt: "Include at least 3 concrete, specific details (numbers, actions, or dialogue)"

### If stories lack emotion:
Add to prompt: "Tap into one of these emotions: frustration, relief, surprise, or recognition. Make the reader feel something."

### If stories exceed word count:
Add to prompt: "CRITICAL: Do not exceed 200 words. If your draft is longer, cut sentences until you're under the limit."

### If endings are weak:
Add to prompt: "The ending must give readers something specific they can do today or a question that makes them reflect."

---

## Integration Suggestions

This template can be integrated into:

- **CLI tools**: Take question as input, output story
- **Web interfaces**: Form with question field, platform selector, generate button
- **IDE extensions**: Generate stories from code comments or documentation
- **CI/CD pipelines**: Auto-generate stories from changelog items
- **Slack/Discord bots**: Generate stories on demand
- **Content calendars**: Batch generate stories for scheduling

---

## Maintenance Notes

Review and update this template:
- When platform algorithms change (e.g., LinkedIn character limits)
- When new best practices emerge in storytelling
- When collecting feedback from actual usage
- When expanding to new platforms

Keep the core principles constant:
- Show, don't tell
- Emotional connection
- Clear structure
- Actionable value

