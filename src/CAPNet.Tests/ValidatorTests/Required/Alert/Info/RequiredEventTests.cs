﻿using CAPNet.Models;
using System.Linq;
using Xunit;

namespace CAPNet
{
    public class RequiredEventTests
    {
        [Fact]
        public void ValidRequiredEvent()
        {
            var info = InfoCreator.CreateValidInfo();
            var eventValidator = new EventRequiredValidator(info);
            Assert.True(eventValidator.IsValid);
            Assert.Equal(0, eventValidator.Errors.Count());
        }

        [Fact]
        public void InvalidRequiredEvent()
        {
            var eventValidator = new EventRequiredValidator(new Info());
            Assert.False(eventValidator.IsValid);
            Assert.Equal(1, eventValidator.Errors.Count());
        }
    }
}
