-> office_confrontation

=== office_confrontation ===
# speaker:Alice
# portrait:Alice_shocked
# bg:Office_BG_01
Ethan?! You call this 'overtime'?

# speaker:Ethan
# portrait:Ethan_nervous
Alice, it's... not what you think.

# speaker:Alice
# portrait:Alice_angry
Not what I think?

# speaker:Alice
# portrait:Alice_angry
Then explain.

# speaker:Ethan
# portrait:Ethan_sad
I was stuck on a deadline. Natalie just stopped by to drop off the files.

# speaker:Alice
# portrait:Alice_angry
Drop off files... in the middle of the night?

# speaker:Ethan
# portrait:Ethan_shocked
Middle of the night? Wait—what time is it?

# speaker:Alice
# portrait:Alice_shocked
It's late. And I'm tired of broken promises.

# speaker:Ethan
# portrait:Ethan_determined
Give me one last chance. I'll prove myself.

* [Trust Ethan] -> trust_path
* [Don't Trust Ethan] -> dont_trust_path

=== trust_path ===
# speaker:Alice
# portrait:Alice_shocked
Alright, Ethan... I'll give you one last chance.

# speaker:Ethan
# portrait:Ethan_determined
You won't regret it.

-> END_scene

=== dont_trust_path ===
# speaker:Alice
# portrait:Alice_angry
No, Ethan. I've heard enough.

# speaker:Ethan
# portrait:Ethan_sad
Alice, please...

-> END_scene

=== END_scene ===
# speaker:Alice
# portrait:Alice_shocked
This conversation is over.
-> DONE
