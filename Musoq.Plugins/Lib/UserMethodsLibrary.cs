﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Musoq.Plugins
{
    public class UserMethodsLibrary
    {
        protected static Group GetParentGroup(Group group, long number)
        {
            var i = 0;
            var parent = @group;

            while (parent.Parent != null && i < number)
            {
                parent = parent.Parent;
                i += 1;
            }

            return parent;
        }

        protected class Occurence
        {
            public int Value { get; private set; }

            public void Increment()
            {
                Value += 1;
            }
        }
    }
}
