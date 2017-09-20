
export namespace ArrayHelpers {

    /**
    * This comparator is used to order objects in an array ascending. It takes
    * the object property to search for as the parameter.
    *
    * @param key    The property to use for comparison.
    * @returns      Returns the compare function
    */
    export function AscendingString(key) {

        // Return an ascending sort function
        return (a, b) => {

            // Order by name ascending
            if (a[key] > b[key]) {
                return 1
            };
            if (a[key] < b[key]) {
                return -1
            };
            return 0;
        };
    };

    /**
    * This does the same as the above function, only it does it descending.
    *
    * @param key    The property to use for comparison.
    * @returns      Returns the compare function
    */
    export function DescendingString(key) {

        // Return an descending sort function
        return (a, b) => {

            // Order by name descending
            if (a[key] > b[key]) {
                return -1
            };

            if (a[key] < b[key]) {
                return 1;
            }
            return 0;
        };
    };

}
