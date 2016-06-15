using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace SMSApi
{
    public class SMSApiProvider
    {
        private string login;
        private string passwordMD5;

        public SMSApiProvider(string login, string passwordMD5)
        {
            this.login = login;
            this.passwordMD5 = passwordMD5;
        }

        /// <summary>
        /// Wysyła sms do odbiorcy
        /// </summary>
        /// <param name="phoneNumber">numer odbiorcy</param>
        /// <param name="content">Zawartośc sms</param>
        /// <param name="authorAlias">Pole nadawcy, działa 'Info',  lub typ wiadomość 'ECO', '2Way'</param>
        /// <returns></returns>
        public int sendSMS(string phoneNumber, string content, string authorAlias)
        {
            var ret = 0;

            try
            {
                SMSApi.Api.Client client = new SMSApi.Api.Client(login);
                client.SetPasswordHash(passwordMD5);

                var smsApi = new SMSApi.Api.SMSFactory(client);

                var result =
                    smsApi.ActionSend()
                        .SetText(content)
                        .SetTo(phoneNumber)
                        .SetSender(authorAlias) //Pole nadawcy, Info,  lub typ wiadomość 'ECO', '2Way'
                        .Execute();

                System.Console.WriteLine("Send: " + result.Count);

                string[] ids = new string[result.Count];

                for (int i = 0, l = 0; i < result.List.Count; i++)
                {
                    if (!result.List[i].isError())
                    {
                        //Nie wystąpił błąd podczas wysyłki (numer|treść|parametry... prawidłowe)
                        if (!result.List[i].isFinal())
                        {
                            //Status nie jest koncowy, może ulec zmianie
                            ids[l] = result.List[i].ID;
                            l++;
                        }
                    }
                }

                System.Console.WriteLine("Get:");
                result =
                    smsApi.ActionGet()
                        .Ids(ids)
                        .Execute();

                foreach (var status in result.List)
                {
                    System.Console.WriteLine("ID: " + status.ID + " NUmber: " + status.Number + " Points:" + status.Points + " Status:" + status.Status + " IDx: " + status.IDx);
                }

                for (int i = 0, l = 0; i < result.List.Count; i++)
                {
                    if (!result.List[i].isError())
                    {
                        var deleted =
                            smsApi.ActionDelete()
                                .Id(result.List[i].ID)
                                .Execute();
                        System.Console.WriteLine("Deleted: " + deleted.Count);
                    }
                }
            }
            catch (SMSApi.Api.ActionException e)
            {
                /**
                 * Błędy związane z akcją (z wyłączeniem błędów 101,102,103,105,110,1000,1001 i 8,666,999,201)
                 * http://www.smsapi.pl/sms-api/kody-bledow
                 */
                //Works even if fails there
                //ret = -1;
                //System.Console.WriteLine(e.Message);
            }
            catch (SMSApi.Api.ClientException e)
            {
                /**
                 * 101 Niepoprawne lub brak danych autoryzacji.
                 * 102 Nieprawidłowy login lub hasło
                 * 103 Brak punków dla tego użytkownika
                 * 105 Błędny adres IP
                 * 110 Usługa nie jest dostępna na danym koncie
                 * 1000 Akcja dostępna tylko dla użytkownika głównego
                 * 1001 Nieprawidłowa akcja
                 */
                ret = -2;
                System.Console.WriteLine(e.Message);
            }
            catch (SMSApi.Api.HostException e)
            {
                /* błąd po stronie servera lub problem z parsowaniem danych
                 * 
                 * 8 - Błąd w odwołaniu
                 * 666 - Wewnętrzny błąd systemu
                 * 999 - Wewnętrzny błąd systemu
                 * 201 - Wewnętrzny błąd systemu
                 * SMSApi.Api.HostException.E_JSON_DECODE - problem z parsowaniem danych
                 */
                System.Console.WriteLine(e.Message);
            }
            catch (SMSApi.Api.ProxyException e)
            {
                // błąd w komunikacji pomiedzy klientem a serverem
                ret = -3;
                System.Console.WriteLine(e.Message);
            }
            return ret;
        }


    }
}
