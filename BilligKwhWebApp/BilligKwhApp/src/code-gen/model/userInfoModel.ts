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
import { LanguageModelSelectListItemModel } from './languageModelSelectListItemModel';


export interface UserInfoModel { 
    name?: string | null;
    email?: string | null;
    languageId?: number;
    mobileNumber?: string | null;
    languages?: Array<LanguageModelSelectListItemModel> | null;
}

