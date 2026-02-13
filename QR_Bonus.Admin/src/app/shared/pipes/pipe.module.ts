import { NgModule } from '@angular/core';
import {CommonModule} from "@angular/common";

import { FilterPipe } from './filter.pipe';
import { SearchPipe } from './search.pipe';
import { ShortNamePipe } from './short-name.pipe';
import { ConvertToZuluPipe } from './convert-to-zulu.pipe';
import { TableRowValuePipe } from './table-row-value.pipe';
import { ImagePipe } from './image.pipe';
import { TranslationPipe } from './translation.pipe';

@NgModule({
  declarations:[FilterPipe, SearchPipe, ShortNamePipe, ConvertToZuluPipe, TableRowValuePipe, ImagePipe, TranslationPipe],
  imports:[CommonModule],
  exports:[FilterPipe, SearchPipe, ShortNamePipe, ConvertToZuluPipe, TableRowValuePipe, ImagePipe, TranslationPipe]
})

export class PipeModule{}
