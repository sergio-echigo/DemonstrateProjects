import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Pipe({
  name: 'imgBase'
})
export class ImgBasePipe implements PipeTransform {

  constructor(private sanitizer: DomSanitizer) {

  }

  transform(base64str: string): any {
    switch(base64str.charAt(0)) {
      case'i':
        return this.sanitizer.bypassSecurityTrustResourceUrl('data:image/png;base64,' + base64str);
      case "/":
        return this.sanitizer.bypassSecurityTrustResourceUrl('data:image/jpg;base64,' + base64str);
      case "R":
        return this.sanitizer.bypassSecurityTrustResourceUrl('data:image/gif;base64,' + base64str);
    }
  }

}
