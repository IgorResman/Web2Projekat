import { Component, OnInit, AfterViewChecked, Input } from '@angular/core';
import { KartaService } from '../services/karta.service';


declare let paypal: any;

@Component({
  selector: 'app-paypal',
  templateUrl: './paypal.component.html',
  styleUrls: ['./paypal.component.css']
})
export class PaypalComponent implements OnInit {
  constructor(private ks: KartaService) { }
  cena: number = 0;


  ngOnInit() { }

  private loadExternalScript(scriptUrl: string) {
    return new Promise((resolve, reject) => {
      const scriptElement = document.createElement('script')
      scriptElement.src = scriptUrl
      scriptElement.onload = resolve
      document.body.appendChild(scriptElement)
    })
  }

  ngAfterViewInit(): void {
    var ks = this.ks;
    this.loadExternalScript("https://www.paypalobjects.com/api/checkout.js").then(() => {
      paypal.Button.render({
        env: 'sandbox',
        client: {
          production: 'sb-icy847118071@business.example.com',
          sandbox: 'AbyLs_0fZpzMyRlwcZe84zzKR4pzdn_Pf7pDuWwn_gNZ3262wE4pjR6Ct1lF3V7mYNORGWNguMBud3RH'
        },
        commit: true,
        payment: function (data, actions) {
          return actions.payment.create({
            payment: {
              transactions: [
                {
                  amount: { total: '5', currency: 'USD' }
                }
              ]
            }

          })
        },
        onAuthorize: function (data, actions) {
          return actions.payment.execute().then(function (payment) {
            // TODO
            // cart: "3VG18081MV5040101"
            // create_time: "2019-08-08T15:51:37Z"
            // id: "PAYID-LVGEKCQ1NP00636Y1192210E"

            ks.SaveTransaction(payment.id).subscribe();
          })
        }
      }, '#paypal-button');
    });
  }

}
