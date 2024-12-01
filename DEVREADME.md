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

## Jump | Fall Animations
How jump was implemented was using a blend tree. I split up the jump frames into 8 animations. They are all played
at different points using a threshold. Using a complicated Map function <br/>
```
        private static float Map(float value, float min1, float max1, float min2, float max2, bool clamp = false)
        {
            var v = min2 + (max2 - min2) * ((value - min1) / (max1 - min1));

            return clamp ? Mathf.Clamp(v, Mathf.Min(min2, max2), Mathf.Max(min2, max2)) : v;
        }

```
<br/>
we are able to map the current y velocity to a point in the blend tree. This allows us to then have the same jump animations
even if we change the jump height. Here is how the Blend Tree 1D Threshold Table looks
| Animation    | Threshold |
| -------- | ------- |
| Jump_7  | 0.4    |
| Jump_6  | 0.45    |
| Jump_5  | 0.5    |
| Jump_4  | 0.55    |
| Jump_3  | 0.75    |
| Jump_2  | 0.85    |
| Jump_1  | 0.95    |
| Jump_0  | 0.99    |

## Movement Stats
I'm keeping track of movement stats for the player character that feel good.

12/01/2024
- Speed: 1.3
- Max Jump Height: 1.5
- Min Jump Hiehgt: 0.2
- Time to Apex: 0.45
- Time to Fall: 0.40

