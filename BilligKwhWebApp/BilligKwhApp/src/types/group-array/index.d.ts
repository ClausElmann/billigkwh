/** Declaration file for group-array
 * NPM: https://www.npmjs.com/package/group-array */

/**
 * Grouping an array by a tag
 * @param {{}[]} arr The aray of object to group
 * @param {string} props The property to first group by
 * @param {string[]} ...args Comma separated list of other properties to group by - following each other.
 */
declare function groupFn(arr: Record<string, unknown>[], props: string, ...args: string[]): Record<string, unknown>;

export = groupFn;
