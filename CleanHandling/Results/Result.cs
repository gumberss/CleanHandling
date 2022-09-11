using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanHandling
{
    [Serializable]
    public struct Result<T, E>
    {
        public bool IsFailure { get; }
        public bool IsSuccess => !IsFailure;

        private readonly E? _error;
        public E? Error => _error;

        private readonly T? _value;
        public T? Value => IsSuccess
            ? _value
            : throw new InvalidOperationException("Result.Value can't be used when an error happened, plese, check IsSuccess or IsFailure property before use Value property");

        public Result(T? value)
        {
            IsFailure = false;
            _value = value;
            _error = default;
        }

        public Result(E? error)
        {
            IsFailure = error != null;
            _value = default;
            _error = error;
        }

        public static implicit operator Result<T, E>(T value)
            => new(value);

        public static implicit operator Result<T, E>(E error)
            => new(error);

        public static implicit operator T?(Result<T, E> result)
            => result.Value;

        public static implicit operator E?(Result<T, E> result)
            => result.Error;

    }

    public class Result
    {
        public static async Task<Result<T?, BusinessException>> Try<T>(Task<T?> func)
        {
            async Task<Result<T?, BusinessException>> tryFunction()
            {
                try
                {
                    return await func;
                }
                catch (Exception ex)
                {
                    return new BusinessException(System.Net.HttpStatusCode.InternalServerError, ex);
                }
            }

            return await Task.Run(() => tryFunction());
        }

        public static async Task<Result<T?, BusinessException>> Try<T>(Func<Task<T?>> func)
        {
            async Task<Result<T?, BusinessException>> tryFunction()
            {
                try
                {
                    return await func();
                }
                catch (Exception ex)
                {
                    return new BusinessException(System.Net.HttpStatusCode.InternalServerError, ex);
                }
            }

            return await Task.Run(() => tryFunction());
        }

        public static async Task<Result<T?, BusinessException>> Try<T>(Func<T?> func)
        {
            Result<T?, BusinessException> tryFunction()
            {
                try
                {
                    return func();
                }
                catch (Exception ex)
                {
                    return new BusinessException(System.Net.HttpStatusCode.InternalServerError, ex);
                }
            }

            return await Task.Run(() => tryFunction());
        }

        public static async Task<Result<bool, BusinessException>> Try(Task func)
        {
            async Task<Result<bool, BusinessException>> tryFunction()
            {
                try
                {
                    await func;
                    return true;
                }
                catch (Exception ex)
                {
                    return new BusinessException(System.Net.HttpStatusCode.InternalServerError, ex);
                }
            }

            return await Task.Run(() => tryFunction());
        }

        public static async Task<Result<bool, BusinessException>> Try(Action act)
        {
            return await Try(() =>
            {
                act();
                return true;
            });
        }

        public static Result<T?, E?> From<T, E>(T? data) 
        {
            return new Result<T?, E?>(data);
        }

        public static Result<T?, BusinessException> From<T>(T? data)
        {
            return new Result<T?, BusinessException>(data);
        }

        public static Result<T?, BusinessException> FromError<T>(BusinessException error)
        {
            return new Result<T?, BusinessException>(error);
        }
    }
}
