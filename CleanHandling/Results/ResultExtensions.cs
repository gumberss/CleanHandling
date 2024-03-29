﻿using System;
using System.Threading.Tasks;

namespace CleanHandling
{
    public static class ResultExtensions
    {
        public async static Task<Result<Tr, Te>> Then<Ti, Tr, Te>
           (this Task<Result<Ti, Te>> input,
           Func<Ti, Task<Result<Tr, Te>>> func) where Te : class
        {
            var result = await input;

            return result.IsFailure
                ? Result.FromError<Tr, Te>(result.Error)
                : await func(result.Value);
        }

        public async static Task<Result<Tr, Te>> Then<Ti, Tr, Te>
             (this Task<Result<Ti, Te>> input,
             Func<Ti, Result<Tr, Te>> func) where Te : class
        {
            var result = await input;

            return result.IsFailure
                ? Result.FromError<Tr, Te>(result.Error)
                : func(result.Value);
        }

        public async static Task<Result<Tr, Te>> Then<Ti, Tr, Te>
            (this Result<Ti, Te> input,
            Func<Ti, Task<Result<Tr, Te>>> func) where Te : class
        {
            var result = input;

            return result.IsFailure
                 ? Result.FromError<Tr, Te>(result.Error)
                 : await func(result.Value);
        }

        public static Result<Tr, Te> Then<Ti, Tr, Te>
          (this Result<Ti, Te> input,
          Func<Ti, Result<Tr, Te>> func) where Te : class
        {
            var result = input;

            return result.IsFailure
                 ? Result.FromError<Tr, Te>(result.Error)
                 : func(result.Value);
        }

        public async static Task<Result<Tr, Te>> Then<Ti, Tr, Te>
            (this Task<Result<Ti, Te>> input,
            Func<Ti, Tr> func) where Te : class
        {
            var result = await input;

            return result.IsFailure
                ? Result.FromError<Tr, Te>(result.Error)
                : Result.From<Tr, Te>(func(result.Value));
        }

        public async static Task<Result<Tr, Te>> When<Ti, Tr, Te>
            (this Task<Result<Ti, Te>> input,
            Func<Ti, bool> condiction,
            Func<Ti, Task<Result<Tr, Te>>> @then,
            Func<Ti, Task<Result<Tr, Te>>>? @else = null) where Te : class
        {
            var result = await input;

            if (result.IsFailure) return result.Error;

            return condiction(result.Value)
                ? await @then(result.Value)
                : @else != null
                    ? await @else(result.Value)
                    : Result.From<Tr, Te>(default);
        }

        public async static Task<Result<Tr, Te>> When<Ti, Tr, Te>
             (this Task<Result<Ti, Te>> input,
             Func<Ti, bool> condiction,
             Func<Ti, Result<Tr, Te>> @then,
             Func<Ti, Result<Tr, Te>>? @else = null) where Te : class
        {
            var result = await input;

            if (result.IsFailure) return result.Error;

            return condiction(result.Value)
                ? @then(result.Value)
                : @else != null
                    ? @else(result.Value)
                    : Result.From<Tr, Te>(default);
        }

        public async static Task<Result<Tr, Te>> When<Ti, Tr, Te>
            (this Result<Ti, Te> input,
            Func<Ti, bool> condiction,
            Func<Ti, Task<Result<Tr, Te>>> @then,
            Func<Ti, Task<Result<Tr, Te>>>? @else = null) where Te : class
        {
            if (input.IsFailure) return input.Error;

            return condiction(input.Value)
                ? await @then(input.Value)
                 : @else != null
                     ? await @else(input.Value)
                     : Result.From<Tr, Te>(default);
        }

        public async static Task<Result<Tr, Te>> When<Ti, Tr, Te>
         (this Task<Result<Ti, Te>> input,
            Func<Ti, bool> condiction,
            Func<Ti, Tr> @then,
            Func<Ti, Tr>? @else = null) where Te : class
        {
            var result = await input;

            if (result.IsFailure)
                return result.Error;

            return condiction(result.Value)
                ? Result.From<Tr, Te>(@then(result.Value))
               : @else != null
                   ? Result.From<Tr, Te>(@else(result.Value))
                   : Result.From<Tr, Te>(default);
        }
    }
}