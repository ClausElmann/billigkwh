import { convertToTitleCase } from "@shared/variables-and-functions/helper-functions";

/**
 * Model to be used everywhere we use server side processing with data tables. Each time, the table queries data, it must use this model for the body object.
 * It's still allowed to add other properties as well...
 **/
export class PagedResultRequest {


   search?: { value: string; regex: string };

   /**
    *  Use this for specifying which columns should be sorted and in which order.
    */
   ordering?: Array<PageResultOrdering>;

   // eslint-disable-next-line @typescript-eslint/no-unused-vars
   public constructor(public page: number, public pageSize: number) { }
}






export class PageResultOrdering {
   /**
    * Ctor.
    * @param propertyName Name of the property/column that should be sorted. This will default be converted to Pascal case
    */
   constructor(public propertyName: string, public direction: "asc" | "desc") {
      this.propertyName = convertToTitleCase(propertyName)
   }
}
