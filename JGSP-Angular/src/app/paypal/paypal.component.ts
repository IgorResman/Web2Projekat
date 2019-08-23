import { Component, OnInit, AfterViewChecked, Input } from '@angular/core';

 
declare let paypal: any;

@Component({
  selector: 'app-paypal',
  templateUrl: './paypal.component.html',
  styleUrls: ['./paypal.component.css']
})
export class PaypalComponent implements OnInit {

  ngOnInit() {
  }
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
  })}

  ngAfterViewInit(): void {
    this.loadExternalScript("https://www.paypalobjects.com/api/checkout.js").then(() => {
      paypal.Button.render({
        env: 'sandbox',
        client: {
          production: 'marko_srb-facilitator@hotmail.rs',
          sandbox: 'AaKKhbw0-y_k74YKRjnPUQwzuqZepCRbQKpy3KHmRh-EFYm5TCNBkUl0naEscvskKOAfaKTLIjxFWD_T'
        },
        commit: true,
        payment: function (data, actions) {
          return actions.payment.create({
            payment: {
              transactions: [
                {
                  amount: { total: '15.00', currency: 'USD' }
                }
              ]
            }
            
          })
        },
        onAuthorize: function(data, actions) {
          return actions.payment.execute().then(function(payment) {
            // TODO
            // cart: "3VG18081MV5040101"
            // create_time: "2019-08-08T15:51:37Z"
            // id: "PAYID-LVGEKCQ1NP00636Y1192210E"
            console.log(payment);
          })
        }
      }, '#paypal-button');
    });
  }
 
}
