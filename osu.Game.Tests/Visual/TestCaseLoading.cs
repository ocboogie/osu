// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.UserInterface;
using OpenTK;

namespace osu.Game.Tests.Visual
{
    [TestFixture]
    public class TestCaseLoading : OsuTestCase
    {
        public override IReadOnlyList<Type> RequiredTypes => new[] { typeof(LoadingAnimation) };

        private readonly LoadingAnimation loading;

        public TestCaseLoading()
        {
            Add(loading = new LoadingAnimation
            {
                Scale = new Vector2(2),
            });

            AddStep("Show", () => loading.Show());
            AddStep("Hide", () => loading.Hide());
        }
    }
}
