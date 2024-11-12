const getCurrentTimeInMS = () => Date.now();

function timeit(fn) {
  return async function (...args) {
    const start = getCurrentTimeInMS();
    const value = await fn(...args);
    const time = getCurrentTimeInMS() - start;

    return { value, time };
  };
}
