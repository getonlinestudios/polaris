# Developer Notes

A collection of notes to future devs, or future self.

## Animations with an intro
An example would be that Mega Man X has intro frames before the looping frames. This is difficult to pull off in Unity animations.
But I've figured something out.

1. In your animator controller create a sub state
2. In that sub state create two animations
3. The first clip will be the intro frames that cannot be looped.
4. The secons will be the loop frames.
5. Connect the "Intro" frames to the "Run" frames
6. To get a seamless transition make sure that the transition has the following settings
- Has Exit Time: `true`
- Exit Time is equal to the whole intro frames (the whole block in the bottom graphic should be blue)
- Fixed Duration: `false`
- Transition Duration: 0
- Transition Offset: 0
- Interruption Source: "Next State"
