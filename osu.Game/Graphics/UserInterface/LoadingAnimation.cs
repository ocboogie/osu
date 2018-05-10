// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using OpenTK;
using OpenTK.Graphics;

namespace osu.Game.Graphics.UserInterface
{
    public class LoadingAnimation : VisibilityContainer
    {
        private readonly SpriteIcon spinner;
        public float CircleSize = 25;
        public float FollowPointSize = 10;

        public List<Color4> CirclesColours => new List<Color4>
        {
            new Color4(136, 102, 238, 255),
            new Color4(255, 85, 204, 255),
            new Color4(102, 204, 255, 255),
            new Color4(255, 204, 34, 255)
        };

        private List<Container> hitCircles = new List<Container> { };
        private List<Circle> approachCircles = new List<Circle> { };
        private List<SpriteIcon> followPoints = new List<SpriteIcon> { };

        public LoadingAnimation()
        {
            int count = CirclesColours.Count;
            FillFlowContainer container;

            Size = new Vector2(1f);

            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Child = container = new FillFlowContainer
            {
                Direction = FillDirection.Horizontal,
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Spacing = Size * CircleSize / 4
            };
            for (int index = 0; index < CirclesColours.Count; index++)
            {
                Color4 circleColour = CirclesColours[index];
                Circle hitCircle;
                Circle approachCircle;

                container.Add(new Container
                {
                    Size = Size * CircleSize,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeChildOffset = new Vector2(0, (index % 2 == 0 ? 1 : -1)),
                    Children = new[]
                    {
                        hitCircle = new Circle
                        {
                            BorderColour = Color4.White,
                            RelativePositionAxes = Axes.Both,
                            RelativeSizeAxes = Axes.Both,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Alpha = 0,
                            BorderThickness = Math.Max(Size.X, Size.Y) * CircleSize / 14,
                            Child = new Box
                            {
                                AlwaysPresent = true,
                                Colour = circleColour,
                                Alpha = 1,
                                RelativeSizeAxes = Axes.Both,
                            },
                        },
                        approachCircle = new Circle
                        {
                            Size = Size * CircleSize * 2,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativePositionAxes = Axes.Both,
                            BorderColour = Color4.White,
                            Alpha = 0,
                            BorderThickness = Math.Max(Size.X, Size.Y) * CircleSize / 15f,
                            Child = new Box
                            {
                                AlwaysPresent = true,
                                Alpha = 0,
                                RelativeSizeAxes = Axes.Both,
                            },
                        }
                    }
                });
                hitCircles.Add(hitCircle);
                approachCircles.Add(approachCircle);

                if (index + 1 != count)
                {
                    SpriteIcon followPoint;
                    container.Add(new Container
                    {
                        Size = Size * FollowPointSize,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Child = followPoint = new SpriteIcon
                        {
                            RelativePositionAxes = Axes.Both,
                            RelativeSizeAxes = Axes.Both,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Alpha = 0,
                            Rotation = index % 2 == 0 ? -45 : 225,
                            Icon = FontAwesome.fa_angle_down
                        },
                    });
                    followPoints.Add(followPoint);
                }
            }
        }

        private const int animation_length = 1600;

        protected override void LoadComplete()
        {
            base.LoadComplete();

            int animationDelay = animation_length / CirclesColours.Count;


            for (int index = 0; index < CirclesColours.Count; index++)
            {
                Circle approachCircle = approachCircles[index];

                hitCircles[index]
                    .Delay(animationDelay * index + 1)
                    .Loop(animation_length * 0.6f, p => p
                        .FadeInFromZero(animation_length * 0.2f)
                        .Then()
                        .ScaleTo(2, animation_length * 0.2f)
                        .FadeOutFromOne(animation_length * 0.2f)
                        .Then()
                        .ScaleTo(1)
                    );
                approachCircles[index]
                    .Delay(animationDelay * index + 1)
                    .Loop(animation_length * 0.8f, p => p
                        .FadeInFromZero(animation_length * 0.2f)
                        .ResizeTo(approachCircle.Size * 0.5f, animation_length * 0.2f)
                        .Then()
                        .FadeOutFromOne()
                        .ResizeTo(approachCircle.Size * 1)
                    );

                if (index + 1 != CirclesColours.Count)
                {
                    followPoints[index]
                        .Delay(animationDelay * index + 1)
                        .Loop(animation_length * 0.5f, p => p
                            .FadeInFromZero(animation_length * 0.1f)
                            .Then(animation_length * 0.3f)
                            .FadeOutFromOne(animation_length * 0.1f)
                        );
                }
            }
        }

        private const float transition_duration = 500;

        protected override void PopIn() => this.FadeIn(transition_duration * 5, Easing.OutQuint);

        protected override void PopOut() => this.FadeOut(transition_duration, Easing.OutQuint);
    }
}
