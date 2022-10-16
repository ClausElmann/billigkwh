/**
 * Color represented in its HEX value as a string.
 */
export enum ColorEnum {
  YELLOW = "Yellow",
  RED = "Red",
  GREEN = "Green",
  BLUE = "Blue",
  WHITE = "White",
  BLACK = "Black",
}

export function ConverStringToColorEnum(color: string): ColorEnum {
  if (color.toLocaleLowerCase() == ColorEnum.YELLOW.toLocaleLowerCase()) return ColorEnum.YELLOW;
  else if (color.toLocaleLowerCase() == ColorEnum.RED.toLocaleLowerCase()) return ColorEnum.RED;
  else if (color.toLocaleLowerCase() == ColorEnum.GREEN.toLocaleLowerCase()) return ColorEnum.GREEN;
  else if (color.toLocaleLowerCase() == ColorEnum.BLUE.toLocaleLowerCase()) return ColorEnum.BLUE;
  else if (color.toLocaleLowerCase() == ColorEnum.WHITE.toLocaleLowerCase()) return ColorEnum.WHITE;
  else return ColorEnum.BLACK;
}
