# Chapter 1: Shattered Anniversary (Test Version)

VAR task1_completed = false
VAR task2_completed = false
VAR task3_completed = false
VAR task4_completed = false
VAR task5_completed = false

=== chapter_1 ===
# Chapter 1: Shattered Anniversary
# speaker:Lina
# portrait:Lina_nervous
# bg:Cafe_Evening

(Scene: Evening. Lina, dressed modestly but neatly, carries a wrapped box. She pauses outside a café window, sees Daniel inside with Mira, laughing. Her heart leaps — at first with joy, then with confusion.)

Lina (thinking, smiling nervously):
"Daniel's here… good, my surprise will work perfectly. He doesn't suspect a thing."

(She steps inside. The bell above the door jingles. She hears Mira's voice, sharp with mockery.)

# speaker:Mira
# portrait:Mira_mocking
Mira (laughing, holding a cheap keychain Daniel gave her):
"Seriously, this is what she buys you? A joke. I wouldn't even use it to hang my dog's collar."

# speaker:Daniel
# portrait:Daniel_snickering
Daniel (snickering, lowering his voice but not enough):
"Don't laugh too loudly… She tries, but honestly, being with her feels like dragging around dead weight. Always tired, always plain. I deserve better."

(The words hit Lina like stones. Her grip tightens on the gift box. She steps forward, voice trembling but clear.)

# speaker:Lina
# portrait:Lina_confrontational
Lina:
"Is that so, Daniel? …This is your idea of working late?"

# speaker:Daniel
# portrait:Daniel_shocked
Daniel (jumps, paling):
"L-Lina?! You—what are you doing here?!"

# speaker:Mira
# portrait:Mira_gleaming
Mira (slowly turning, eyes gleaming):
"Oh. So the charity case shows up. Perfect timing."

# speaker:Lina
# portrait:Lina_steady
Lina (takes a shaky breath, forcing steadiness):
"Yes, it's from me. A watch. Bought after months of cleaning shifts. Cheap, maybe. But at least I bought it with my own hands… not by clinging to someone else's wallet."

# speaker:Daniel
# portrait:Daniel_flustered
Daniel (flustered, raising his voice to hide shame):
"Don't start your drama here! You're embarrassing me in front of everyone!"

# speaker:Mira
# portrait:Mira_possessive
Mira (smirks, takes Daniel's arm possessively):
"Embarrassing you? She's embarrassing herself. Look at her clothes, her hair… She doesn't belong in places like this."

# speaker:Lina
# portrait:Lina_breaking
Lina (her voice breaking, throws the gift box onto the table; the lid slides open, the watch glimmers under the café lights):
"I thought today mattered. I thought we mattered. But you know what? You're right. I don't belong here. Not with you."

(Silence. Customers glance their way. Mira, annoyed, grabs a cup of hot coffee and flicks it toward Lina. She gasps, trying to dodge, but the liquid splashes across her hand.)

# speaker:Lina
# portrait:Lina_pain
Lina (cries out in pain, clutching her wrist):
"Ahh—it burns!"

# speaker:Mira
# portrait:Mira_cold
Mira (coldly):
"Ugly, clumsy, broke, and now even scarred. Who would ever want trash like you?"

(Lina staggers back, face pale. She forces herself not to cry in front of them. She turns and stumbles out into the street, clutching her burned hand. Outside, Emma spots her and runs over.)

# speaker:Emma
# portrait:Emma_concerned
Emma:
"Lina! What happened?! Your hand—oh my god, it's red and blistering! Who did this?!"

# speaker:Lina
# portrait:Lina_weak
Lina (weakly, eyes glassy):
"…Maybe she was right, Emma. Maybe I really am… useless."

# speaker:Emma
# portrait:Emma_fierce
Emma (fierce, shaking her shoulders):
"Don't. Say. That. Not after everything you've done for him. You gave up your studies, you worked your fingers raw, you loved him more than he deserved. You're not useless—he is."

# speaker:Lina
# portrait:Lina_whisper
Lina (voice barely a whisper):
"…Then why does it hurt so much to hear them say it?"

# speaker:Emma
# portrait:Emma_gentle
Emma (softens, pulling her into an embrace):
"Because you cared. But now it's time to stop caring about them and start caring about you. Come with me, Lina. Let me show you what I see when I look at you."

(Emma pulls Lina gently down the street, toward a boutique lit with warm golden lights.)

* [Follow Emma] 
    -> ch1_task1_outfit

=== ch1_task1_outfit ===
# Task 1: Outfit Selection
# speaker:Emma
# portrait:Emma_encouraging
# bg:Boutique_Interior
(Scene: Inside the boutique. Emma has helped Lina clean her burned hand and now they're looking at clothing options.)

Emma (encouraging):
"See? You look amazing in that outfit. Now let's find the perfect hairstyle to match."

* (Professional) -> ch1_task2_intro
* (Wry) -> ch1_task2_intro  
* (Guarded) -> ch1_task2_intro

=== ch1_task2_intro ===
# Task 2: Hair Styling
# speaker:Emma
# portrait:Emma_encouraging
Emma:
"Your hair is beautiful, Lina. Let's just give it a little something extra to make you feel confident."

* (Natural) -> ch1_task3_intro
* (Bold) -> ch1_task3_intro
* (Minimal) -> ch1_task3_intro

=== ch1_task3_intro ===
# Task 3: Makeup
# speaker:Emma
# portrait:Emma_encouraging
Emma:
"A little makeup can work wonders. Let's enhance your natural beauty."

* (Elegant) -> ch1_task4_intro
* (Trendy) -> ch1_task4_intro
* (Classic) -> ch1_task4_intro

=== ch1_task4_intro ===
# Task 4: Accessories
# speaker:Emma
# portrait:Emma_encouraging
Emma:
"Accessories can make or break an outfit. Let's choose something that speaks to your personality."

* (Confident) -> ch1_task5_intro
* (Playful) -> ch1_task5_intro
* (Determined) -> ch1_task5_intro

=== ch1_task5_intro ===
# Task 5: Finalize Look
# speaker:Emma
# portrait:Emma_encouraging
Emma:
"Now let's put it all together. Take a look in the mirror and see the new you."

* (Complete Look) -> ch1_complete

=== ch1_complete ===
# Sau khi hoàn thành tất cả tasks
# speaker:Lina
# portrait:Lina_beautiful
# bg:Boutique_Mirror
(Lina looks at herself in the mirror, and for the first time in a long time, she sees someone she can be proud of.)

Lina (softly, to her reflection):
"I... I look beautiful."

# speaker:Emma
# portrait:Emma_beaming
Emma (beaming):
"You ARE beautiful, Lina. You always were. You just needed to see it for yourself."

# speaker:Lina
# portrait:Lina_grateful
Lina (turning to Emma, eyes shining):
"Thank you, Emma. For everything."

# speaker:Emma
# portrait:Emma_encouraging
Emma:
"Now let's go show the world what you're made of. Ready to start your new chapter?"

# speaker:Lina
# portrait:Lina_confident
Lina (standing tall, confidence restored):
"More than ready."

-> chapter_2_intro

=== chapter_2_intro ===
# Transition to Chapter 2
# bg:Black_Fade
(Scene fades to black, then opens on a new evening...)

# Chapter 2: A Deal with a Stranger
# speaker:Emma
# portrait:Emma_urgent
# bg:Night_Market
(Scene: Night market street. Neon glows on rain-dark pavement. Emma hustles Lina toward a small boutique with warm lighting.)

Emma (urgent but gentle):
"First, we clean and dress that burn. Then we fix your look—not for him, for you."

-> chapter_2

=== chapter_2 ===
# Chapter 2 content sẽ được thêm sau
# Đây là nơi để tiếp tục story
