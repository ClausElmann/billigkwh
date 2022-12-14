/**
 * BilligKwh API Documentation
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: v1
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */
import { PagedResultSearchFilter } from './pagedResultSearchFilter';
import { PageResultOrdering } from './pageResultOrdering';


export interface LanguageResourcesDto { 
    page?: number;
    pageSize?: number;
    search?: PagedResultSearchFilter;
    ordering?: Array<PageResultOrdering> | null;
    searchResourceName?: string | null;
    searchResourceValue?: string | null;
}

