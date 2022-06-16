import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';

@Pipe({
  name: 'localDateTime'
})
export class LocalDateTimePipe implements PipeTransform {

  transform(date: string) : string {
    return (moment(date, "YYYY-MM-DDTHH:mm:ss")).format("MM-DD-YYYY");
  }

}
