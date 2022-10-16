import { Pipe, PipeTransform } from "@angular/core";
import { convertNewlinesToHtmlTags } from "@shared/variables-and-functions/helper-functions";

/**
 * Pipe for processing new lines in a text and convert them to HTML <br> tags
 */
@Pipe({ name: "toBrTags" })
export class BiToBrTagsPipe implements PipeTransform {
  transform(value: string) {
    if (value) {
      return convertNewlinesToHtmlTags(value);
    }
  }
}
