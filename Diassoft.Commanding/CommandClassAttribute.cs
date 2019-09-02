using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Commanding
{
    /// <summary>
    /// Represents an attribute to set a Class as a Command Container 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CommandClassAttribute: System.Attribute
    {
        /// <summary>
        /// Represents the command class lifetime
        /// </summary>
        /// <remarks>The lifetime can be <see cref="InstanceLifetimes.Singleton"/>, <see cref="InstanceLifetimes.Scoped"/> or <see cref="InstanceLifetimes.Transient"/>. Those settings will define whe the instance of the command class is initialized.</remarks>
        public InstanceLifetimes Lifetime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandClassAttribute"/>
        /// </summary>
        /// <remarks>The default value for the <see cref="Lifetime"/> property is set to <see cref="InstanceLifetimes.Scoped"/> if not informed on the constructor</remarks>
        public CommandClassAttribute(): this(InstanceLifetimes.Scoped) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandClassAttribute"/>
        /// </summary>
        /// <param name="lifetime">The <see cref="InstanceLifetimes">Instance Lifetime</see>, <see cref="InstanceLifetimes.Singleton"/>, <see cref="InstanceLifetimes.Scoped"/> or <see cref="InstanceLifetimes.Transient"/></param>
        public CommandClassAttribute(InstanceLifetimes lifetime)
        {
            Lifetime = lifetime;
        }

    }
}
