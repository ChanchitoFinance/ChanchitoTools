# Platform Specifications

This file contains specifications for different social media platforms. **LinkedIn is the default** unless changed by the developer.

---

## LinkedIn (DEFAULT)

### Platform Characteristics:
- **Primary Audience**: Professionals, developers, business people
- **Tone**: Professional but conversational and human
- **Optimal Length**: 150-200 words (sweet spot: 180-190)
- **Character Limit**: ~3,000 characters (plenty of room)
- **Engagement**: Longer posts often perform better than short ones

### Content Guidelines:
- **Opening**: Strong, value-forward statement
- **Body**: Story-based, show expertise through experience
- **Ending**: Call to action or valuable insight
- **Format**: Short paragraphs with line breaks

### Best Practices:
- ✅ Professional yet personal voice
- ✅ Share lessons and insights
- ✅ Use concrete examples from work
- ✅ Ask thought-provoking questions
- ✅ Encourage meaningful discussion
- ✅ Bold text for emphasis (1-2 uses)
- ✅ Emoji sparingly (0-2 per post)

### Avoid:
- ❌ Overly casual language
- ❌ Excessive self-promotion
- ❌ Clickbait tactics
- ❌ Multiple hashtags (max 3-5)
- ❌ Too many emoji
- ❌ Purely theoretical content

### Example Format:
```
[Strong opening statement about value]

[First paragraph: Setup the scene]

[Second paragraph: The turning point or action]

[Third paragraph: The realization or outcome]

[Call to action or valuable takeaway]
```

---

## Twitter/X

**Note**: Requires significant adaptation from the LinkedIn format.

### Platform Characteristics:
- **Character Limit**: 280 characters (unless premium: 10,000)
- **Optimal Length**: 150-280 characters for best engagement
- **Tone**: More casual, punchy, direct
- **Format**: Single short paragraph or thread

### Content Guidelines (Single Tweet):
- **Opening**: One punchy line (the insight)
- **Optional**: One concrete detail
- **Ending**: Question or call to action

### Thread Format (Recommended):
1. **Tweet 1**: Bold statement with the main insight (hook)
2. **Tweet 2-3**: Brief story moment or concrete example
3. **Tweet 4**: Actionable takeaway or question

### Best Practices:
- ✅ Be concise and punchy
- ✅ Use short sentences
- ✅ More casual tone
- ✅ Can use more emoji (2-4)
- ✅ Encourage retweets and replies
- ✅ Use line breaks for readability

### Avoid:
- ❌ Long paragraphs
- ❌ Too formal
- ❌ Excessive hashtags (max 1-2)
- ❌ Thread abuse (keep to 4-5 tweets max)

---

## Instagram

**Note**: Requires visual component (image/graphic) + caption.

### Platform Characteristics:
- **Caption Length**: Up to 2,200 characters
- **Optimal Length**: 125-150 words
- **Tone**: More personal, visual, inspiring
- **Format**: Short paragraphs with line breaks and emoji

### Content Guidelines:
- **Opening**: Emotional hook or intriguing question
- **Body**: Brief story (100 words max)
- **Ending**: Call to action with emoji
- **Hashtags**: Place at end or in first comment (10-15 relevant ones)

### Best Practices:
- ✅ More emotional and personal
- ✅ Visual language (describe what you see)
- ✅ Use emoji liberally (3-6)
- ✅ Line breaks for readability
- ✅ Question in caption to drive comments
- ✅ Tag relevant accounts
- ✅ Include hashtags for discoverability

### Story Adaptation:
- Focus more on the emotional moment
- Less technical detail
- More universal appeal
- Shorter, punchier sentences

---

## Facebook

### Platform Characteristics:
- **Optimal Length**: 200-300 words
- **Tone**: Friendly, conversational, community-focused
- **Format**: Longer paragraphs acceptable
- **Engagement**: Questions and polls work well

### Content Guidelines:
- **Opening**: Relatable statement or question
- **Body**: Story with more detail (can be longer)
- **Ending**: Strong call to action (comment, share, discuss)

### Best Practices:
- ✅ More conversational
- ✅ Can be longer and more detailed
- ✅ Encourage discussion and sharing
- ✅ Use questions throughout
- ✅ React to comments actively
- ✅ Emoji acceptable (2-4)

---

## Platform Comparison Table

| Feature | LinkedIn | Twitter/X | Instagram | Facebook |
|---------|----------|-----------|-----------|----------|
| **Word Count** | 180-200 | 40-70 (or thread) | 125-150 | 200-300 |
| **Tone** | Professional + Human | Punchy + Casual | Personal + Visual | Conversational |
| **Emoji Use** | 0-2 | 2-4 | 3-6 | 2-4 |
| **Paragraphs** | Short with breaks | Very short | Short with breaks | Can be longer |
| **Technical Detail** | Moderate | Minimal | Very minimal | Low to moderate |
| **Hashtags** | 3-5 | 1-2 | 10-15 | 3-5 |
| **Call to Action** | Reflection/Question | Question/Engage | Comment/Question | Comment/Share |

---

## Adaptation Guidelines

When converting from LinkedIn (default) to other platforms:

### LinkedIn → Twitter/X:
1. Extract the core insight (opening)
2. Choose ONE moment from the story
3. Make it ultra-concise
4. End with a question
5. Consider a 3-4 tweet thread for full story

### LinkedIn → Instagram:
1. Make the opening more emotional
2. Condense story to 80-100 words
3. Add more sensory/visual details
4. Use more emoji
5. Ensure it complements a visual

### LinkedIn → Facebook:
1. Make tone slightly more casual
2. Can expand story slightly (250-300 words)
3. Add more conversational elements
4. Strengthen engagement question at end

---

## Default Configuration

```json
{
  "default_platform": "LinkedIn",
  "word_limits": {
    "LinkedIn": 200,
    "Twitter": 70,
    "Instagram": 150,
    "Facebook": 300
  },
  "tone_profiles": {
    "LinkedIn": "professional_conversational",
    "Twitter": "punchy_casual",
    "Instagram": "personal_visual",
    "Facebook": "friendly_community"
  },
  "emoji_ranges": {
    "LinkedIn": [0, 2],
    "Twitter": [2, 4],
    "Instagram": [3, 6],
    "Facebook": [2, 4]
  }
}
```

---

## Developer Instructions

### Setting Default Platform:

```python
# In your configuration or as a parameter
DEFAULT_PLATFORM = "LinkedIn"  # Change this to switch default

# Platform can be overridden by user input
def generate_story(question, platform=DEFAULT_PLATFORM):
    # Use platform specs from this file
    pass
```

### Platform Validation:

```python
SUPPORTED_PLATFORMS = ["LinkedIn", "Twitter", "Instagram", "Facebook"]

def validate_platform(platform: str) -> bool:
    if platform not in SUPPORTED_PLATFORMS:
        raise ValueError(f"Unsupported platform. Use: {SUPPORTED_PLATFORMS}")
    return True
```

### Dynamic Prompt Adjustment:

```python
def get_platform_specs(platform: str) -> dict:
    """Load specs for specific platform"""
    specs = {
        "LinkedIn": {
            "max_words": 200,
            "tone": "professional but conversational",
            "emoji_max": 2,
            "paragraph_style": "short with line breaks"
        },
        "Twitter": {
            "max_words": 70,
            "tone": "punchy and casual",
            "emoji_max": 4,
            "paragraph_style": "very short, single focus"
        },
        # ... other platforms
    }
    return specs.get(platform, specs["LinkedIn"])
```

---

## Future Platform Additions

When adding new platforms, document:

1. **Character/word limits**
2. **Audience demographics and expectations**
3. **Optimal tone and style**
4. **Formatting preferences**
5. **Engagement patterns**
6. **Content restrictions**
7. **Example adaptations**

### Potential Additions:
- Medium (long-form)
- Reddit (discussion-focused)
- Threads (Twitter/X competitor)
- TikTok (video scripts with storytelling)
- YouTube Community Posts
- Newsletter formats

---

## Quality Assurance

Before publishing on any platform, verify:

- [ ] Content fits within platform's word/character limit
- [ ] Tone matches platform conventions
- [ ] Emoji usage appropriate for platform
- [ ] Formatting optimized for platform
- [ ] Call to action suitable for platform culture
- [ ] Hashtags (if any) follow platform best practices
- [ ] Content complies with platform guidelines

