/**
 * Enum defining language
 */
export enum BiLanguageId {
  DK = 1,
  SE = 2,
  EN = 3,
  FI = 4,
  NO = 5,
}

export function getLanguageTranslationKeyByLanguageId(id: BiLanguageId) {
  switch (id) {
    case BiLanguageId.DK: return "shared.Danish";
    case BiLanguageId.SE: return "shared.Swedish";
    case BiLanguageId.EN: return "shared.English";
    case BiLanguageId.FI: return "shared.Finnish";
    case BiLanguageId.NO: return "shared.Norwegian";
  }
}

/**
 * Enum defining country
 */

export enum BiCountryId {
  Default = 0,
  DK = 1,
  SE = 2,
  EN = 3,
  FI = 4,
  NO = 5
}
