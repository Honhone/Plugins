using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimationController
{
    
    public static class Maybe
    {
        //?obsolete with .? ? ?        
        public static TResult With<TInput, TResult>(this TInput input, Func<TInput, TResult> eval)
            where TInput : class where TResult : class
        {
            if (input == null) return null;
            return eval(input);
        }

        /// <summary>
        /// Returns failure value if input is null, else returns value
        /// </summary>
        public static TResult Return<TInput, TResult>(this TInput input, Func<TInput, TResult> eval, TResult failure)
            where TInput : class
        {
            if (input == null) return failure;
            return eval(input);
        }

        /// <summary>
        /// Boolean returns whether something exist or not
        /// </summary>
        public static bool Exist<TInput>(this TInput input) where TInput : class
        {
            return input != null;
        }
        /// <summary>
        /// If not null and condition is met, do thing else null
        /// </summary>
        public static TInput If<TInput>(this TInput input, Predicate<TInput> eval) where TInput : class
        {
            if (input == null) return null;
            return eval(input) ? input : null;
        }

        /// <summary>
        /// If not null, do something
        /// </summary>
        public static TInput Act<TInput>(this TInput input, Action<TInput> action)
            where TInput : class
        {
            if (input == null) return null;
            action(input);
            return (input);
        }
    }
///Usage:
//var foo = bar.With(x => x.Zoo)
//             .If(x => x.[condition])
//            .Act(Console.WriteLine)
//             .Return(x => [anything success], [anything failure]);

}
