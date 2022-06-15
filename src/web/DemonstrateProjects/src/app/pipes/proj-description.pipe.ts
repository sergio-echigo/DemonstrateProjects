import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'projDescription'
})
export class ProjDescriptionPipe implements PipeTransform {

  transform(desc : string): string {
    if (desc.length > 45)
      return desc.substring(0, 45) + " (...)";

    return desc;
  }

}
