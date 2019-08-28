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
  // @Input()
  // public cena: number;

  // addScript: boolean = false;
  // paypalLoad: boolean = true;

  // finalAmount: number = this.cena;

  // paypalConfig = {
  //   env: 'sandbox',
  //   client: {
  //     sandbox: 'marko_srb-facilitator@hotmail.rs',
  //     production: 'access_token$sandbox$rxmzt3yrz365v9cy$c304ff9fac12f48c4426b7697aeb2550'
  //   },
  //   commit: true,
  //   payment: (data, actions) => {
  //     return actions.payment.create({
  //       payment: {
  //         transactions: [
  //           { amount: { total: this.finalAmount, currency: 'INR' } }
  //         ]
  //       }
  //     });
  //   },
  //   onAuthorize: (data, actions) => {
  //     return actions.payment.execute().then((payment) => {
  //       alert("Uspesno ste obavili placanje!");
  //     })
  //   }
  // };

  // ngAfterViewChecked(): void {
  //   if (!this.addScript) {
  //     this.addPaypalScript().then(() => {
  //       paypal.Button.render(this.paypalConfig, '#paypal-checkout-btn');
  //       this.paypalLoad = false;
  //     })
  //   }
  // }

  // addPaypalScript() {
  //   this.addScript = true;
  //   return new Promise((resolve, reject) => {
  //     let scripttagElement = document.createElement('script');    
  //     scripttagElement.src = 'https://www.paypalobjects.com/api/checkout.js';
  //     scripttagElement.onload = resolve;
  //     document.body.appendChild(scripttagElement);
  //   })
  // }


  private loadExternalScript(scriptUrl: string) {
    return new Promise((resolve, reject) => {
      const scriptElement = document.createElement('script')
      scriptElement.src = scriptUrl
      scriptElement.onload = resolve
      document.body.appendChild(scriptElement)
    })
  }

  ngAfterViewInit(): void {
    //console.log(this.cenaKarte)
    // var c = this.cena; //cena karte u dolarima
    var ks = this.ks;
    this.loadExternalScript("https://www.paypalobjects.com/api/checkout.js").then(() => {
      paypal.Button.render({
        env: 'sandbox',
        client: {
          production: 'sb-897zd113414@business.example.com',
          sandbox: 'AVpqV92cOs1IF7B7djyG_T7QMb1032VDJMofRn1LirX4Mre2caD4IqrxKydbQMXkJK-MkKt8jkOVHnUh'
        },
        commit: true,
        payment: function (data, actions) {
          return actions.payment.create({
            payment: {
              transactions: [
                {
                  amount: { total: '15', currency: 'USD' }
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
            // console.log(payment);
            // console.log(l);
          })
        }
      }, '#paypal-button');
    });
  }

}
