# Writing Guidelines: Show, Don't Tell

## Core Principle: Create Scenes, Not Explanations

The difference between good and great stories is **showing** readers an experience instead of **telling** them about it.

---

## Show vs Tell: Technical Examples

### Example 1: Finding DDD Concepts

❌ **TELLING:**
"To find domain concepts, you should talk to domain experts and observe their language patterns."

✅ **SHOWING:**
"The analyst said 'batch' three times in two minutes. I started writing down every noun she used. By the end of our coffee, I had fifteen concepts—none of which appeared in our codebase."

---

### Example 2: Avoiding Overcomplicated Models

❌ **TELLING:**
"Developers often add unnecessary complexity when they try to predict future needs."

✅ **SHOWING:**
"He added five interfaces for a feature that changed twice a year. 'Future-proofing,' he called it. Six months later, we deleted four of them. Nobody had time to understand the maze."

---

### Example 3: Modeling Change Over Time

❌ **TELLING:**
"You need to consider how values evolve when designing your data model."

✅ **SHOWING:**
"The bug report said 'incorrect score.' But when we looked at the database, it was right—for now. We'd been overwriting history. Every update erased what came before."

---

## Techniques for Emotional Connection

### 1. Use Sensory Details
**Purpose**: Make the scene vivid and real

- ❌ "The meeting was frustrating"
- ✅ "Three hours in the conference room. Whiteboard full of boxes and arrows. Nobody could explain what 'processing' actually meant."

### 2. Show Physical Actions
**Purpose**: Ground abstract concepts in tangible behavior

- ❌ "We decided to simplify"
- ✅ "She drew a line through half the diagram. 'What if we just... don't?'"

### 3. Include Internal Reactions
**Purpose**: Create empathy and recognition

- ❌ "The solution was obvious"
- ✅ "I stared at the code. How had I missed something so simple?"

### 4. Use Dialogue (Sparingly)
**Purpose**: Make characters and moments feel immediate

- ❌ "The expert provided clarity"
- ✅ "'Wait,' the nurse interrupted, 'we never discharge on weekends.' Our entire model fell apart."

---

## Connecting to Feelings

### Universal Developer Emotions

Your stories should tap into experiences your audience recognizes:

**Frustration:**
- Dealing with overcomplicated code
- Miscommunication between teams
- Technical debt catching up

**Relief:**
- Finding a simple solution
- Finally understanding a concept
- Deleting unnecessary code

**Curiosity:**
- Discovering new patterns
- Learning from mistakes
- Understanding how systems really work

**Recognition:**
- "I've been there"
- "I've made that mistake"
- "I've felt that confusion"

---

## Creating "A-ha" Moments

Every story needs a turning point—a moment where something clicks.

### Structure of an A-ha Moment:

1. **Setup**: Establish the confusion or problem
2. **Catalyst**: Something specific happens (conversation, observation, failure)
3. **Realization**: The character understands differently
4. **Change**: How they act/think differently now

### Example:

```
Setup: "Our 'Order' class had twenty-three properties. Nobody could explain what half of them did."

Catalyst: "Then I watched a warehouse worker for an hour. She talked about 'picks,' 'packs,' and 'ships' like they were completely separate things."

Realization: "We'd been modeling three different concepts as one giant entity."

Change: "We split it. Suddenly, the code made sense."
```

---

## Voice and Tone Guidelines

### ✅ Do:
- Write like you're talking to a colleague over coffee
- Use "I," "we," "you" freely
- Keep sentences short and punchy
- Vary sentence length for rhythm
- Use contractions (it's, don't, we're)
- Be conversational but professional

### ❌ Don't:
- Use academic language
- Write long, complex sentences
- Over-explain technical terms
- Sound preachy or superior
- Use corporate jargon
- Be overly formal

---

## Character Creation

### Who Lives in Your Stories?

Characters make abstract concepts relatable. Use:

**First-Person (You/I):**
- "I spent three days refactoring..."
- "The first time I heard about bounded contexts..."

**Second-Person (Reader):**
- "You've seen this before..."
- "Picture this: You open a file with 47 methods..."

**Third-Person (Colleague/Expert):**
- "The product manager laughed..."
- "My teammate asked a question that changed everything..."

**Anonymous/Universal:**
- "A junior developer once told me..."
- "The best architect I know..."

### Character Details (Use Sparingly):
- Don't overdo character description
- One or two specific details maximum
- Focus on actions and words, not appearance
- Make them relatable to your audience

---

## Scene-Setting Checklist

When crafting your story scene, include:

- [ ] **Where**: Specific location (office, call, whiteboard, code review)
- [ ] **Who**: Character(s) involved
- [ ] **What happened**: Concrete action or event
- [ ] **The problem/tension**: What wasn't working
- [ ] **The moment**: When something shifted
- [ ] **The feeling**: Emotional response

---

## Common Pitfalls to Avoid

### 1. Over-Explaining
❌ "Event sourcing is a pattern where instead of storing current state, you store a sequence of events that led to that state, which allows you to..."

✅ "We deleted the 'current_score' column. Now we just replay the events. It felt backwards—until we needed to audit every change."

### 2. Multiple Messages
- Focus on ONE insight per story
- Multiple lessons dilute impact
- Save other insights for other stories

### 3. Weak Endings
- Don't trail off
- Don't just restate the obvious
- Give something valuable to take away

### 4. Too Much Setup
- Jump into the scene quickly
- Cut unnecessary context
- Trust readers to fill in gaps

### 5. Abstract Language
Replace abstract terms with concrete imagery:
- "complexity" → "a 300-line method"
- "communication issues" → "three teams using three different words for the same thing"
- "technical debt" → "code we're afraid to touch"

---

## Final Polish

### Read Aloud Test
Read your story out loud. If it sounds:
- Awkward or stilted → simplify language
- Boring or slow → add specific details
- Confusing → clarify the sequence
- Preachy → add humility or vulnerability

### Emotion Check
Ask: "Would this make someone *feel* something?"
- Recognition? ("I've been there")
- Surprise? ("I never thought of it that way")
- Motivation? ("I want to try that")
- Relief? ("I'm not the only one")

### Value Check
Ask: "If someone reads this and closes LinkedIn, what do they take away?"
- A new perspective?
- A practical tip?
- A question to reflect on?
- Permission to do things differently?

---

## Remember

**People don't remember facts. They remember stories.**

Your goal isn't to teach—it's to make someone *see* the concept through a real moment. When they see it, they'll remember it.

