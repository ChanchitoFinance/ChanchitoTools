# Writing Guidelines: Show, Don't Tell

## Core Principle: Focus on Feelings and Experiences, Not Technical Details

The difference between good and great stories is **showing** readers how something FEELS and the human IMPACT it has, rather than explaining technical mechanics or implementation details.

---

## Show vs Tell: Focus on Human Experience

### Example 1: Finding Real Domain Understanding

❌ **TELLING (Technical Focus):**
"To find domain concepts, you should talk to domain experts and observe their language patterns."

✅ **SHOWING (Experience Focus):**
"The analyst said 'batch' three times in two minutes. I felt lost—like I was speaking a different language. I started writing down every noun she used. By the end of our coffee, I had fifteen concepts—none of which appeared in our codebase. The relief was immediate."

---

### Example 2: The Weight of Unnecessary Complexity

❌ **TELLING (Technical Focus):**
"Developers often add unnecessary complexity when they try to predict future needs."

✅ **SHOWING (Experience Focus):**
"He added five interfaces for a feature that changed twice a year. 'Future-proofing,' he called it. Six months later, we deleted four of them. The weight lifted immediately—nobody had time to understand the maze he'd built."

---

### Example 3: The Frustration of Lost Context

❌ **TELLING (Technical Focus):**
"You need to consider how values evolve when designing your data model."

✅ **SHOWING (Experience Focus):**
"The bug report said 'incorrect score.' But when we looked at the database, it was right—for now. The frustration was real. We'd been overwriting history. Every update erased what came before, leaving us with no way to explain the journey."

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
- Prefer "you" and conversational framing over personal anecdotes
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
- Invent personal past jobs, university projects, or client stories

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

- [ ] **Where**: Specific location (office, call, whiteboard, code review) or clearly hypothetical ("imagine this")
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

## Using Project Context for Authentic Stories

### Why Project Context Matters
When generating stories, reference actual patterns from your current project to create more authentic and relatable content:

- **Real Entity Names**: If your project has `User`, `Order`, `Payment` entities, use them
- **Actual Business Domain**: Reference the real business problems you're solving
- **Genuine Pain Points**: Draw from actual developer frustrations in the project
- **Authentic Scenarios**: Create stories that could realistically happen in your context

### How to Incorporate Project Context
- Look at your entity models and domain concepts
- Consider the real business rules and constraints you're working with
- Think about actual user scenarios and edge cases
- Reference real naming conventions and patterns from your codebase

This makes stories feel genuine rather than generic, while still focusing on the emotional and experiential aspects rather than technical implementation details.

## Story Approaches: Past Experiences vs Future Scenarios

### Choose Your Approach Based on Impact
You can tell stories about past experiences or create future scenarios - both have their place:

### APPROACH 1: Past Experience Stories
**When to Use**: When you have concrete examples from actual project code or real scenarios
**Structure**: Problem encountered → Journey to solution → Lesson learned
**Code Reference**: When writing about past experiences, scan and reference real code patterns from the project to make stories authentic and specific.

### APPROACH 2: Future Scenario Stories  
**When to Use**: When you want to create urgency and show consequences of inaction
**Structure**: Hypothetical situation → Crisis builds → Urgent call to action

### Varied Opening Patterns (Don't Always Use "Until...")

**Future Scenario Openings** (mix these up):
- "Your code may work perfectly now... until..."
- "Picture this scenario..."
- "Imagine this situation..."
- "Here's what happens when..."
- "The moment you realize..."
- "You think you're safe until..."

**Past Experience Openings**:
- "I was debugging a..."
- "The bug report said..."
- "We spent weeks..."
- "The moment I realized..."
- "Last week, I encountered..."
- "The error message was simple..."

### Key Elements for Suspenseful Future Scenarios:

#### 1. **Build Suspense with "Until..."**
- "Your code may work perfectly now... until..."
- "Everything seems fine... until suddenly..."
- "This feels safe... but here's what happens when..."

#### 2. **Create Anxiety with Time Pressure**
- "suddenly," "overnight," "without warning"
- "The trap is already set... you just haven't stepped in it yet"
- "You won't realize the problem... until it's too late"

#### 3. **Paint the Crisis Vividly**
- "Every business method has database calls buried inside"
- "Your validation logic is scattered across seven different classes"
- "Testing becomes impossible because everything depends on everything"

#### 4. **Show the Emotional Cost**
- "Two weeks of refactoring," "impossible to test," "limited every future decision"
- "The frustration was overwhelming," "The panic was real"
- "Every change felt like walking through a maze"

#### 5. **End with Urgency**
- "That's why separation matters now, before you need it"
- "Fix this before your next major feature request"
- "Your future self will thank you for this decision today"

### Suspense and Anxiety Language Patterns:
- **Suspense Builders**: "Your code may work perfectly now... until..."
- **Anxiety Triggers**: "suddenly," "overnight," "without warning"
- **Crisis Amplifiers**: "Every business method," "All your validation," "Testing becomes impossible"
- **Cost Emphasizers**: "Two weeks of refactoring," "impossible to test," "limited every future decision"
- **Urgency Drivers**: "That's why separation matters now," "Fix this before," "Your future self will thank you"

## Remember

**People don't remember facts. They remember stories.**

Your goal isn't to teach—it's to make someone *feel* the concept through a vivid, human experience. If you invent, frame it as hypothetical—not as your personal past. Use your project's real context to make stories authentic and relatable.

