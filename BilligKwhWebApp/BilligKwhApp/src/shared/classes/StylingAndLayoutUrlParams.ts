import { BiLanguageId } from "@globals/enums/BiLanguageAndCountryId";

/**
 * Class used for having a single place to define the available URL parameters for different iFrame modules
 */
export class StylingAndLayoutParams {
  /**
   * Returns the available styling parameters for the "Driftstatus"-iFrame module
   */
  public static getDriftStatusIframeStylingParams(languageId: BiLanguageId) {
    if (languageId === BiLanguageId.DK) {
      return [
        "<span class='t--bold'>Titel skriftstørrelse: </span> &titleSize=19",
        "<span class='t--bold'>Titel skrifttype:</span> &titleFont=Arial",
        "<span class='t--bold'>Titel skriftfarve:</span> &titleColor=000000",
        "<span class='t--bold'>Store bogstaver i titlen:</span> &headerCaps=1",
        "<span class='t--bold'>Header skriftstørrelse:</span> &headerSize=21",
        "<span class='t--bold'>Header skrifttype:</span> &headerFont=calibri",
        "<span class='t--bold'>Header skriftfarve:</span> &headerFontColor=ffffff",
        "<span class='t--bold'>Baggrundsfarve i header:</span> &headerColor=81899D",
        "<span class='t--bold'>Baggrundsbillede i header:</span> &headerBg=URL",
        "<span class='t--bold'>Custom font (Google Font):</span> &fontUrl=https%3A%2F%2Ffonts.googleapis.com%2Fcss%3Ffamily%3DRoboto",
        "<span class='t--bold'>Custom CSS:</span> &cssUrl=https://style.css",
        "<span class='t--bold'>Brødtekst skrifttype:</span> &font=Arial",
        "<span class='t--bold'>Brødtekst skriftstørrelse:</span> &fontSize=12",
        "<span class='t--bold'>Brødtekst skriftfarve:</span> &fontColor=000000",
        "<span class='t--bold'>Overskrift:</span> &header=Driftstatus",
        "<span class='t--bold'>Baggrundsfarve:</span> &mainColor=f0f0f0",
        "<span class='t--bold'>Kantfarver:</span> &borderColor=d4d4d4"
      ];
    } else if (languageId === BiLanguageId.SE) {
      return [
        "<span class='t--bold'>Titel Teckenstorlek: </span> &titleSize=19",
        "<span class='t--bold'>Titel - Teckensnitt:</span> &titleFont=Arial",
        "<span class='t--bold'>Titel - Textfärg:</span> &titleColor=000000",
        "<span class='t--bold'>Titel - Större bokstäver:</span> &headerCaps=1",
        "<span class='t--bold'>Header - Teckenstorlek:</span> &headerSize=21",
        "<span class='t--bold'>Header - Teckensnitt:</span> &headerFont=calibri",
        "<span class='t--bold'>Header - Teckenfärg:</span> &headerFontColor=ffffff",
        "<span class='t--bold'>Header - Bakgrundsfärg:</span> &headerColor=81899D",
        "<span class='t--bold'>Header - Bakgrundsbild: </span> &headerBg=URL",
        "<span class='t--bold'>Anpassad font (Google Font):</span> &fontUrl=https%3A%2F%2Ffonts.googleapis.com%2Fcss%3Ffamily%3DRoboto",
        "<span class='t--bold'>Anpassad CSS:</span> &cssUrl=https://style.css",
        "<span class='t--bold'>Brödtext teckensnitt:</span> &font=Arial",
        "<span class='t--bold'>Brödtext teckenstorlek:</span> &fontSize=12",
        "<span class='t--bold'>Brödtext teckenfärg:</span> &fontColor=000000",
        "<span class='t--bold'>Titel:</span> &header=Driftstatus",
        "<span class='t--bold'>Bakgrundsfärg:</span> &mainColor=f0f0f0",
        "<span class='t--bold'>Kantfärger:</span> &borderColor=d4d4d4"
      ];
    } else if (languageId === BiLanguageId.EN) {
      return [
        "<span class='t--bold'>Title font size: </span> &titleSize=19",
        "<span class='t--bold'>Title font:</span> &titleFont=Arial",
        "<span class='t--bold'>Title font color:</span> &titleColor=000000",
        "<span class='t--bold'>Title - capital letters:</span> &headerCaps=1",
        "<span class='t--bold'>Header font size:</span> &headerSize=21",
        "<span class='t--bold'>Header font:</span> &headerFont=calibri",
        "<span class='t--bold'>Header font color:</span> &headerFontColor=ffffff",
        "<span class='t--bold'>Background color in header:</span> &headerColor=81899D",
        "<span class='t--bold'>Background image in header:</span> &headerBg=URL",
        "<span class='t--bold'>Custom font (Google Font):</span> &fontUrl=https%3A%2F%2Ffonts.googleapis.com%2Fcss%3Ffamily%3DRoboto",
        "<span class='t--bold'>Custom CSS:</span> &cssUrl=https://style.css",
        "<span class='t--bold'>Body text font:</span> &font=Arial",
        "<span class='t--bold'>Body text font size:</span> &fontSize=12",
        "<span class='t--bold'>Body text font color:</span> &fontColor=000000",
        "<span class='t--bold'>Heading:</span> &header=Driftstatus",
        "<span class='t--bold'>Background color:</span> &mainColor=f0f0f0",
        "<span class='t--bold'>Border colors:</span> &borderColor=d4d4d4"
      ];
    } else if (languageId === BiLanguageId.FI) {
      return [
        "<span class='t--bold'>Otsikon fonttikoko: </span> &titleSize=19",
        "<span class='t--bold'>Otsikon fontti:</span> &titleFont=Arial",
        "<span class='t--bold'>Otsikon fontin väri:</span> &titleColor=000000",
        "<span class='t--bold'>Otsikko – isot kirjaimet:</span> &headerCaps=1",
        "<span class='t--bold'>Ylätunnisteen fonttikoko:</span> &headerSize=21",
        "<span class='t--bold'>Ylätunnisteen fontti:</span> &headerFont=calibri",
        "<span class='t--bold'>Ylätunnisteen fontin väri:</span> &headerFontColor=ffffff",
        "<span class='t--bold'>Ylätunnisteen taustaväri:</span> &headerColor=81899D",
        "<span class='t--bold'>Ylätunnisteen taustakuva:</span> &headerBg=URL",
        "<span class='t--bold'>Mukautettu fontti (Google-fontti):</span> &fontUrl=https%3A%2F%2Ffonts.googleapis.com%2Fcss%3Ffamily%3DRoboto",
        "<span class='t--bold'>Mukautettu CSS:</span> &cssUrl=https://style.css",
        "<span class='t--bold'>Leipätekstin fontti:</span> &font=Arial",
        "<span class='t--bold'>Leipätekstin fonttikoko:</span> &fontSize=12",
        "<span class='t--bold'>Leipätekstin fontin väri:</span> &fontColor=000000",
        "<span class='t--bold'>Ylätunniste:</span> &header=Driftstatus",
        "<span class='t--bold'>Taustaväri:</span> &mainColor=f0f0f0",
        "<span class='t--bold'>Reunan värit:</span> &borderColor=d4d4d4"
      ];
    } else if (languageId === BiLanguageId.NO) {
      return [
        "<span class='t--bold'>Tittelskriftstørrelse: </span> &titleSize=19",
        "<span class='t--bold'>Tittelskrifttype:</span> &titleFont=Arial",
        "<span class='t--bold'>Tittelskriftfarge:</span> &titleColor=000000",
        "<span class='t--bold'>Tittel – store bokstaver:</span> &headerCaps=1",
        "<span class='t--bold'>Skriftstørrelse for topptekst:</span> &headerSize=21",
        "<span class='t--bold'>Skrifttype for topptekst:</span> &headerFont=calibri",
        "<span class='t--bold'>Skriftfarge for topptekst:</span> &headerFontColor=ffffff",
        "<span class='t--bold'>Bakgrunnsfarge i topptekst:</span> &headerColor=81899D",
        "<span class='t--bold'>Bakgrunnsbilde i topptekst:</span> &headerBg=URL",
        "<span class='t--bold'>Egendefinert skrifttype (Google Font):</span> &fontUrl=https%3A%2F%2Ffonts.googleapis.com%2Fcss%3Ffamily%3DRoboto",
        "<span class='t--bold'>Egendefinert CSS:</span> &cssUrl=https://style.css",
        "<span class='t--bold'>Skrifttype for brødtekst:</span> &font=Arial",
        "<span class='t--bold'>Skriftstørrelse for brødtekst:</span> &fontSize=12",
        "<span class='t--bold'>Skriftfarge for brødtekst:</span> &fontColor=000000",
        "<span class='t--bold'>Overskrift:</span> &header=Driftstatus",
        "<span class='t--bold'>Driftsstatus:</span> &mainColor=f0f0f0",
        "<span class='t--bold'>Kantfarver:</span> &borderColor=d4d4d4"
      ];
    }
  }

  /**
   * Returns the available layout parameters for the "Driftstatus"-iFrame module
   */
  public static getDriftStatusIframeLayoutParams(languageId: BiLanguageId) {
    if (languageId === BiLanguageId.DK) {
      return [
        "<span class='t--bold'>Vis kanter(borders):</span> &borders=0",
        "<span class='t--bold'>Vis alt indhold:</span> &displayAllContent=1",
        "<span class='t--bold'>Skjul profilenavne ved visning af alt indhold:</span> &hideProfiles=1",
        "<span class='t--bold'>Skjul statusindikator:</span> &hideIndicator=1",
        "<span class='t--bold'>URL til anden side (kun link vil vises):</span> &permaLinkURL=https://www.blueidea.dk",
        "<span class='t--bold'>Vis indikator ved permalink:</span> &showIndicator=1",
        "<span class='t--bold'>Skjul header:</span> &hideHeader=1",
        "<span class='t--bold'>Skjul datoer:</span> &hideDates=1",
        "<span class='t--bold'>Vis afsluttede som aktive:</span> &completedAsActive=1",
        "<span class='t--bold'>Omvendt rækkefølge:</span> &reversedOrder=1",
        "<span class='t--bold'>Vis kun interne:</span> &internalOnly=1"
      ];
    } else if (languageId === BiLanguageId.SE) {
      return [
        "<span class='t--bold'>Visa kanter(borders):</span> &borders=0",
        "<span class='t--bold'>Visa allt innehåll:</span> &displayAllContent=1",
        "<span class='t--bold'>Dölj profilnamn vid visning av allt innehåll:</span> &hideProfiles=1",
        "<span class='t--bold'>Dölj statusindikator:</span> &hideIndicator=1",
        "<span class='t--bold'>URL till andra sidan (bara länken visas):</span> &permaLinkURL=https://www.blueidea.se",
        "<span class='t--bold'>Visa indikator vid permalänk:</span> &showIndicator=1",
        "<span class='t--bold'>Dölj header:</span> &hideHeader=1",
        "<span class='t--bold'>Dölj datum:</span> &hideDates=1",
        "<span class='t--bold'>Visa slutförd som aktiv:</span> &completedAsActive=1",
        "<span class='t--bold'>Omvänd radordning:</span> &reversedOrder=1",
        "<span class='t--bold'>Visa bara interna:</span> &internalOnly=1"
      ];
    } else if (languageId === BiLanguageId.EN) {
      return [
        "<span class='t--bold'>Show borders:</span> &borders=0",
        "<span class='t--bold'>Show all content:</span> &displayAllContent=1",
        "<span class='t--bold'>Hide profile names when viewing all content:</span> &hideProfiles=1",
        "<span class='t--bold'>Hide status indicator:</span> &hideIndicator=1",
        "<span class='t--bold'>Second page URL(only link will be displayed):</span> &permaLinkURL=https://www.blueidea.dk",
        "<span class='t--bold'>Show indicator at permalink:</span> &showIndicator=1",
        "<span class='t--bold'>Hide header:</span> &hideHeader=1",
        "<span class='t--bold'>Hide dates:</span> &hideDates=1",
        "<span class='t--bold'>Show completed as active:</span> &completedAsActive=1",
        "<span class='t--bold'>Reverse order:</span> &reversedOrder=1",
        "<span class='t--bold'>Show internal only:</span> &internalOnly=1"
      ];
    } else if (languageId === BiLanguageId.FI) {
      return [
        "<span class='t--bold'>Näytä reunat:</span> &borders=0",
        "<span class='t--bold'>Näytä kaikki sisältö:</span> &displayAllContent=1",
        "<span class='t--bold'>Piilota profiilien nimet, kun kaikki sisältö näytetään:</span> &hideProfiles=1",
        "<span class='t--bold'>Piilota tilailmaisin:</span> &hideIndicator=1",
        "<span class='t--bold'>Toisen sivun URL-osoite (vain linkki näytetään):</span> &permaLinkURL=https://www.blueidea.dk",
        "<span class='t--bold'>Näytä ilmaisin ikilinkissä:</span> &showIndicator=1",
        "<span class='t--bold'>Piilota ylätunniste:</span> &hideHeader=1",
        "<span class='t--bold'>Piilota päivämäärät:</span> &hideDates=1",
        "<span class='t--bold'>Näytä valmis aktiivisena:</span> &completedAsActive=1",
        "<span class='t--bold'>Käänteinen järjestys:</span> &reversedOrder=1",
        "<span class='t--bold'>Näytä vain sisäinen:</span> &internalOnly=1"
      ];
    } else if (languageId === BiLanguageId.NO) {
      return [
        "<span class='t--bold'>Vis kantlinjer:</span> &borders=0",
        "<span class='t--bold'>Vis alt innhold:</span> &displayAllContent=1",
        "<span class='t--bold'>Skjul profilnavn ved visning av alt innhold:</span> &hideProfiles=1",
        "<span class='t--bold'>Skjul statusindikator:</span> &hideIndicator=1",
        "<span class='t--bold'>URL-adresse for andre side (kun kobling vil vises):</span> &permaLinkURL=https://www.blueidea.dk",
        "<span class='t--bold'>Vis indikator ved permakobling:</span> &showIndicator=1",
        "<span class='t--bold'>Skjul topptekst:</span> &hideHeader=1",
        "<span class='t--bold'>Skjul datoer:</span> &hideDates=1",
        "<span class='t--bold'>Vis fullført som aktiv:</span> &completedAsActive=1",
        "<span class='t--bold'>Omvendt rekkefølge:</span> &reversedOrder=1",
        "<span class='t--bold'>Vis kun interne:</span> &internalOnly=1"
      ];
    }
  }

  /**
   * Returns the available styling parameters for the "Tilmeldings"-iFrame module
   */
  public static getSubscriptionIframeStylingParams(languageId: BiLanguageId, forStdReceiverModule?: boolean) {
    let returnValue: Array<string>;

    if (languageId === BiLanguageId.DK) {
      returnValue = [
        "<span class='t--bold'>Baggrundsfarve:</span> &backgroundColor=ffffff",
        "<span class='t--bold'>Farve på knapper:</span> &btnColor=6a6a6a",
        "<span class='t--bold'>Mouseover farve på knapper:</span> &btnHoverColor=fafafa",
        "<span class='t--bold'>Farve på tilbageknap:</span> &backColor=000000",
        "<span class='t--bold'>Skriftfarve:</span> &fontColor=000000",
        "<span class='t--bold'>Skriftstørrelse:</span> &fontSize=19",
        "<span class='t--bold'>Skriftstørrelse i inputs:</span> &inputFontSize=17",
        "<span class='t--bold'>Skrifttype:</span> &fontFamily=Arial",
        "<span class='t--bold'>Skrifttype i inputs:</span> &inputFontFamily=calibri",
        "<span class='t--bold'>Skriftfarve på knapper:</span> &btnFontColor=ffffff",
        "<span class='t--bold'>Skriftstørrelse på knapper:</span> &btnFontSize=17",
        "<span class='t--bold'>Header skriftstørrelse:</span> &headerSize=19",
        "<span class='t--bold'>Header skriftfarve:</span> &headerFontColor=ffffff",
        "<span class='t--bold'>Header baggrundsfarve:</span> &headerColor=ffffff",
        "<span class='t--bold'>Custom font (Google Font) url:</span> &fontUrl=https%3A%2F%2Ffonts.googleapis.com%2Fcss%3Ffamily%3DRoboto",
        "<span class='t--bold'>Custom CSS:</span> &cssUrl=https://style.css",
        "<span class='t--bold'>Disclaimer skriftfarve:</span> &disclaimerTextColor=fafafa",
        "<span class='t--bold'>Farve på ydre kant:</span> &borderColor=c8c8c8",
        "<span class='t--bold'>Skjul ydre kant:</span> &borders=0"

      ];
      if (!forStdReceiverModule)
        returnValue.push(`<span class="t--bold">Skriftfarve på "Tak for din tilmelding"-teksten:</span> &thankYouTextColor=81899D`);
    } else if (languageId === BiLanguageId.SE) {
      returnValue = [
        "<span class='t--bold'>Bakgrundsfärg:</span> &backgroundColor=ffffff",
        "<span class='t--bold'>Knapper - Färg:</span> &btnColor=6a6a6a",
        "<span class='t--bold'>Knappar - Musöverfärg:</span> &btnHoverColor=fafafa",
        "<span class='t--bold'>Färg på bakåtknappen:</span> &backColor=000000",
        "<span class='t--bold'>Teckenfärg:</span> &fontColor=000000",
        "<span class='t--bold'>Teckenstorlek:</span> &fontSize=19",
        "<span class='t--bold'>Inputs - Teckenstorlek:</span> &inputFontSize=17",
        "<span class='t--bold'>Teckensnitt:</span> &fontFamily=Arial",
        "<span class='t--bold'>Inputs - Teckensnitt:</span> &inputFontFamily=calibri",
        "<span class='t--bold'>Knappar - Teckenfärg:</span> &btnFontColor=ffffff",
        "<span class='t--bold'>Knappar - Teckenstorlek:</span> &btnFontSize=17",
        "<span class='t--bold'>Header - Teckenstorlek:</span> &headerSize=19",
        "<span class='t--bold'>Header - Teckenfärg:</span> &headerFontColor=ffffff",
        "<span class='t--bold'>Header - Bakgrundsfärg:</span> &headerColor=ffffff",
        "<span class='t--bold'>Anpassad font (Google Font) url:</span> &fontUrl=https%3A%2F%2Ffonts.googleapis.com%2Fcss%3Ffamily%3DRoboto",
        "<span class='t--bold'>Anpassad CSS:</span> &cssUrl=https://style.css",
        "<span class='t--bold'>Disclaimer teckenfärg:</span> &disclaimerTextColor=fafafa",
        "<span class='t--bold'>Färg på ytterkanten:</span> &borderColor=c8c8c8",
        "<span class='t--bold'>Dölj ytterkanten:</span> &borders=0"
      ];
      if (!forStdReceiverModule) returnValue.push(`<span class="t--bold"> Teckenfärg på "Tack för"-texten:</span> &thankYouTextColor=81899D`);
    } else if (languageId === BiLanguageId.EN) {
      returnValue = [
        "<span class='t--bold'>Background color:</span> &backgroundColor=ffffff",
        "<span class='t--bold'>Button colors:</span> &btnColor=6a6a6a",
        "<span class='t--bold'>Button mouseover color:</span> &btnHoverColor=fafafa",
        "<span class='t--bold'>Back button color:</span> &backColor=000000",
        "<span class='t--bold'>Font color:</span> &fontColor=000000",
        "<span class='t--bold'>Font size:</span> &fontSize=19",
        "<span class='t--bold'>Font size in inputs:</span> &inputFontSize=17",
        "<span class='t--bold'>Font:</span> &fontFamily=Arial",
        "<span class='t--bold'>Font type in inputs:</span> &inputFontFamily=calibri",
        "<span class='t--bold'>Button font color:</span> &btnFontColor=ffffff",
        "<span class='t--bold'>Button font size:</span> &btnFontSize=17",
        "<span class='t--bold'>Header font size:</span> &headerSize=19",
        "<span class='t--bold'>Header font color:</span> &headerFontColor=ffffff",
        "<span class='t--bold'>Header background color:</span> &headerColor=ffffff",
        "<span class='t--bold'>Custom font (Google Font) url:</span> &fontUrl=https%3A%2F%2Ffonts.googleapis.com%2Fcss%3Ffamily%3DRoboto",
        "<span class='t--bold'>Custom CSS:</span> &cssUrl=https://style.css",
        "<span class='t--bold'>Disclaimer text color:</span> &disclaimerTextColor=fafafa",
        "<span class='t--bold'>Outer border color:</span> &borderColor=c8c8c8",
        "<span class='t--bold'>Hide outer border:</span> &borders=0"
      ];
      if (!forStdReceiverModule)
        returnValue.push(`<span class="t--bold">Font color on the "Thank you for your subscription!"-text:</span> &thankYouTextColor=81899D`);
    } else if (languageId === BiLanguageId.FI) {
      returnValue = [
        "<span class='t--bold'>Taustaväri:</span> &backgroundColor=ffffff",
        "<span class='t--bold'>Painikkeiden värit:</span> &btnColor=6a6a6a",
        "<span class='t--bold'>Painikkeen väri hiirellä osoitettaessa:</span> &btnHoverColor=fafafa",
        "<span class='t--bold'>Paluupainikkeen väri:</span> &backColor=000000",
        "<span class='t--bold'>Fontin väri:</span> &fontColor=000000",
        "<span class='t--bold'>Fonttikoko:</span> &fontSize=19",
        "<span class='t--bold'>Syötteiden fonttikoko:</span> &inputFontSize=17",
        "<span class='t--bold'>Fontti:</span> &fontFamily=Arial",
        "<span class='t--bold'>Syötteiden fonttityyppi:</span> &inputFontFamily=calibri",
        "<span class='t--bold'>Painikefontin väri:</span> &btnFontColor=ffffff",
        "<span class='t--bold'>Painikefontin koko:</span> &btnFontSize=17",
        "<span class='t--bold'>Ylätunnisteen fonttikoko:</span> &headerSize=19",
        "<span class='t--bold'>Ylätunnisteen fontin väri:</span> &headerFontColor=ffffff",
        "<span class='t--bold'>Ylätunnisteen taustaväri:</span> &headerColor=ffffff",
        "<span class='t--bold'>Mukautettu fontti (Google-fontti):</span> &fontUrl=https%3A%2F%2Ffonts.googleapis.com%2Fcss%3Ffamily%3DRoboto",
        "<span class='t--bold'>Mukautettu CSS</span> &cssUrl=https://style.css",
        "<span class='t--bold'>Vastuuvapauslausekkeen tekstin väri:</span> &disclaimerTextColor=fafafa",
        "<span class='t--bold'>Väri ulkoreunassa:</span> &borderColor=c8c8c8",
        "<span class='t--bold'>Piilota ulkoreuna:</span> &borders=0"
      ];
      if (!forStdReceiverModule)
        returnValue.push(`<span class="t--bold">Fontin väri ”Kiitos tilauksesta!” ‑tekstissä:</span> &thankYouTextColor=81899D`);
    } else if (languageId === BiLanguageId.NO) {
      returnValue = [
        "<span class='t--bold'>Bakgrunnsfarge:</span> &backgroundColor=ffffff",
        "<span class='t--bold'>Knappefarger:</span> &btnColor=6a6a6a",
        "<span class='t--bold'>Knappefarge ved peker over:</span> &btnHoverColor=fafafa",
        "<span class='t--bold'>Farge på tilbakeknapp:</span> &backColor=000000",
        "<span class='t--bold'>Skriftfarge:</span> &fontColor=000000",
        "<span class='t--bold'>Skriftstørrelse:</span> &fontSize=19",
        "<span class='t--bold'>Skriftstørrelse i inndata:</span> &inputFontSize=17",
        "<span class='t--bold'>Skrifttype:</span> &fontFamily=Arial",
        "<span class='t--bold'>Skrifttype i inndata:</span> &inputFontFamily=calibri",
        "<span class='t--bold'>Knappeskriftfarge:</span> &btnFontColor=ffffff",
        "<span class='t--bold'>Knappeskriftstørrelse:</span> &btnFontSize=17",
        "<span class='t--bold'>Skriftstørrelse for topptekst:</span> &headerSize=19",
        "<span class='t--bold'>Skriftfarge for topptekst:</span> &headerFontColor=ffffff",
        "<span class='t--bold'>Bakgrunnsfarge for topptekst:</span> &headerColor=ffffff",
        "<span class='t--bold'>Egendefinert skrifttype (Google Font):</span> &fontUrl=https%3A%2F%2Ffonts.googleapis.com%2Fcss%3Ffamily%3DRoboto",
        "<span class='t--bold'>Farge på ytterkant:</span> &borderColor=c8c8c8",
        "<span class='t--bold'>Skjul ytterkanten:</span> &borders=0"
      ];
      if (!forStdReceiverModule)
        returnValue.push(`<span class="t--bold">Skriftfarge på teksten "Takk for at du vil abonnere!":</span> &thankYouTextColor=81899D`);
    }

    return returnValue;
  }

  /**
   * Returns the available layout parameters for the "Tilmeldings"-iFrame module
   */
  public static getSubscriptionIframeLayoutParams(languageId: BiLanguageId, forStdReceiverModule?: boolean) {
    if (languageId === BiLanguageId.DK) {
      return [
        "<span class='t--bold'>Skjul tilbageknap:</span> &hideBackBtn=1",
        "<span class='t--bold'>Skjul e-mail input:</span> &hideEmailInput=1",
        "<span class='t--bold'>Vis input til fastnet nummer:</span> &showLandline=1"
      ].concat(
        forStdReceiverModule
          ? [
            "<span class='t--bold'>Deaktiver kodeverifikation:</span> &disableAuth=1",
            "<span class='t--bold'>Modul kun for specifikke grupper (kommasepareret liste af gruppe ID'er):</span> &groupIds=1234,5678,9123"
          ]
          : []
      );
    } else if (languageId === BiLanguageId.SE) {
      return [
        "<span class='t--bold'>Dölj tillbakaknappen:</span> &hideBackBtn=1",
        "<span class='t--bold'>Dölj e-post input:</span> &hideEmailInput=1",
        "<span class='t--bold'>Visa input till fastnätsnummer:</span> &showLandline=1"
      ].concat(
        forStdReceiverModule
          ? [
            "<span class='t--bold'>Stäng av kodverifikation:</span> &disableAuth=1",
            "<span class='t--bold'>Modul endast för specifika grupper (kommaseparerad lista med grupp-ID):</span> &groupIds=1234,5678,9123"
          ]
          : []
      );
    } else if (languageId === BiLanguageId.EN) {
      return [
        "<span class='t--bold'>Hide back button:</span> &hideBackBtn=1",
        "<span class='t--bold'>Hide e-mail input:</span> &hideEmailInput=1",
        "<span class='t--bold'>Show input for landline:</span> &showLandline=1"
      ].concat(
        forStdReceiverModule
          ? [
            "<span class='t--bold'>Deactivate code verification:</span> &disableAuth=1",
            "<span class='t--bold'>Module for specific groups only (comma separated list of group IDs):</span> &groupIds=1234,5678,9123"
          ]
          : []
      );
    } else if (languageId === BiLanguageId.FI) {
      return [
        "<span class='t--bold'>Piilota paluupainike:</span> &hideBackBtn=1",
        "<span class='t--bold'>Piilota sähköpostin syöte:</span> &hideEmailInput=1",
        "<span class='t--bold'>Näytä lankapuhelinlinjan syöte:</span> &showLandline=1"
      ].concat(
        forStdReceiverModule
          ? [
            "<span class='t--bold'>Poista koodin vahvistus käytöstä:</span> &disableAuth=1",
            "<span class='t--bold'>Module for specific groups only (comma separated list of group IDs):</span> &groupIds=1234,5678,9123"
          ]
          : []
      );
    } else if (languageId === BiLanguageId.NO) {
      return [
        "<span class='t--bold'>Skjul tilbakeknapp:</span> &hideBackBtn=1",
        "<span class='t--bold'>Skjul inndata for e-post:</span> &hideEmailInput=1",
        "<span class='t--bold'>Vis inndata for fasttelefon:</span> &showLandline=1"
      ].concat(
        forStdReceiverModule
          ? [
            "<span class='t--bold'>Deaktiver kodeverifisering:</span> &disableAuth=1",
            "<span class='t--bold'>Modul bare for spesifikke grupper (komma-separert liste over gruppe-ID-er):</span> &groupIds=1234,5678,9123"
          ]
          : []
      );
    }
  }

  public static getSubscriptionIFrameFeatureParams(languageId: BiLanguageId) {
    if (languageId === BiLanguageId.DK) {
      return [
        "<span class='t--bold'>Sprog (2 karakterer, ISO 639-1 format):</span> &language=da",
        "<span class='t--bold'>Deaktiver kodeverifikation:</span> &disableAuth=1",
        "<span class='t--bold'>Deaktiver operatøropslag:</span> &disableLookup=1",
        `<span class="t--bold">Adresseopslag kun på erhvervsadresser:</span> &onlyBusinessLookup=1`,
        `<span class="t--bold">Adresseopslag kun på privatadresser:</span> &onlyPrivateLookup=1`
      ];
    } else if (languageId === BiLanguageId.SE) {
      return [
        "<span class='t--bold'>Språk (2 tecken, ISO 639-1 format):</span> &language=se",
        "<span class='t--bold'>Stäng av kodverifikation:</span> &disableAuth=0",
        "<span class='t--bold'>Deaktivera operatörssökning - visar endast data som är registrerad inte via operatörerna:</span> &disableLookup=1",
        `<span class="t--bold">Skapa adressökning på företagsadresser:</span> &onlyBusinessLookup=1`,
        `<span class="t--bold">Skapa adressökning på privatadresser:</span> &onlyPrivateLookup=1`
      ];
    } else if (languageId === BiLanguageId.EN) {
      return [
        "<span class='t--bold'>Language (2 characters, ISO 639-1 format):</span> &language=en",
        "<span class='t--bold'>Deactivate code verification:</span> &disableAuth=1",
        "<span class='t--bold'>Disable operator lookups:</span> &disableLookup=1",
        `<span class="t--bold">Address lookup on business addresses only:</span> &onlyBusinessLookup=1`,
        `<span class="t--bold">Address lookup on private addresses only:</span> &onlyPrivateLookup=1`
      ];
    } else if (languageId === BiLanguageId.FI) {
      return [
        "<span class='t--bold'>Kieli (2 merkkiä, ISO 639-1 muoto):</span> &language=fi",
        "<span class='t--bold'>Poista koodin vahvistus käytöstä:</span> &disableAuth=1",
        "<span class='t--bold'>Poista operaattorihaut käytöstä:</span> &disableLookup=1",
        `<span class="t--bold">Osoitehaku vain yritysosoitteissa:</span> &onlyBusinessLookup=1`,
        `<span class="t--bold">Osoitehaku vain yksityisissä osoitteissa:</span> &onlyPrivateLookup=1`
      ];
    } else if (languageId === BiLanguageId.NO) {
      return [
        "<span class='t--bold'>Språk (2 tegn, ISO 639-1 format):</span> &language=no",
        "<span class='t--bold'>Deaktiver kodeverifisering:</span> &disableAuth=1",
        "<span class='t--bold'>Deaktiver operatøroppslag:</span> &disableLookup=1",
        `<span class="t--bold">Adresseoppslag kun på kontoradresser:</span> &onlyBusinessLookup=1`,
        `<span class="t--bold">Adresseoppslag kun på private adresser:</span> &onlyPrivateLookup=1`
      ];
    }
  }
}
