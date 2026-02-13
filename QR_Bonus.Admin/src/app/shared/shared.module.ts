import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { OverlayModule } from '@angular/cdk/overlay';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TranslateModule } from '@ngx-translate/core';
import { PerfectScrollbarModule } from "ngx-perfect-scrollbar";
import { ClickOutsideModule } from 'ng-click-outside';

import { AutocompleteModule } from './components/autocomplete/autocomplete.module';
import { PipeModule } from 'app/shared/pipes/pipe.module';

//COMPONENTS
import { FooterComponent } from "./footer/footer.component";
import { NavbarComponent } from "./navbar/navbar.component";
import { HorizontalMenuComponent } from './horizontal-menu/horizontal-menu.component';
import { VerticalMenuComponent } from "./vertical-menu/vertical-menu.component";
import { CustomizerComponent } from './customizer/customizer.component';
import { NotificationSidebarComponent } from './notification-sidebar/notification-sidebar.component';

import { ConfirmActionDialogComponent } from './components/popups/confirm-action-dialog/confirm-action-dialog.component';

//DIRECTIVES
import { ToggleFullscreenDirective } from "./directives/toggle-fullscreen.directive";
import { SidebarLinkDirective } from './directives/sidebar-link.directive';
import { SidebarDropdownDirective } from './directives/sidebar-dropdown.directive';
import { SidebarAnchorToggleDirective } from './directives/sidebar-anchor-toggle.directive';
import { SidebarDirective } from './directives/sidebar.directive';
import { TopMenuDirective } from './directives/topmenu.directive';
import { TopMenuLinkDirective } from './directives/topmenu-link.directive';
import { TopMenuDropdownDirective } from './directives/topmenu-dropdown.directive';
import { TopMenuAnchorToggleDirective } from './directives/topmenu-anchor-toggle.directive';
import { DatetimePickerComponent } from './control-value-accessors/datetime-picker/datetime-picker.component';
import { CheckboxComponent } from './control-value-accessors/checkbox/checkbox.component';
import { DropdownComponent } from './control-value-accessors/dropdown/dropdown.component';
import { NumberInputComponent } from './control-value-accessors/number-input/number-input.component';
import { PasswordInputComponent } from './control-value-accessors/password-input/password-input.component';
import { SwitchToggleComponent } from './control-value-accessors/swithc-toggle/switch-toggle.component';
import { TextAreaComponent } from './control-value-accessors/text-area/text-area.component';
import { TextInputComponent } from './control-value-accessors/text-input/text-input.component';
import { PaginationComponent } from './components/pagination/pagination.component';

import { NgSelectModule } from '@ng-select/ng-select';
import { OnlyRealNumbersDirective } from './directives/only-real-numbers.directive';
import { DynamicFiltersComponent } from './components/dynamic-filters/dynamic-filters.component';
import { DynamicFiltersDateTimeComponent } from './components/dynamic-filters/controls/dynamic-filters-date-time/dynamic-filters-date-time.component';
import { DynamicFiltersNumberComponent } from './components/dynamic-filters/controls/dynamic-filters-number/dynamic-filters-number.component';
import { DynamicFiltersSelectComponent } from './components/dynamic-filters/controls/dynamic-filters-select/dynamic-filters-select.component';
import { DynamicFiltersSwitchToggleComponent } from './components/dynamic-filters/controls/dynamic-filters-switch-toggle/dynamic-filters-switch-toggle.component';
import { DynamicFiltersTextComponent } from './components/dynamic-filters/controls/dynamic-filters-text/dynamic-filters-text.component';
import { DynamicTableComponent } from './components/dynamic-table/dynamic-table.component';
import { ImageComponent } from './control-value-accessors/image/image.component';
import { LightboxModule } from 'ngx-lightbox';
import { QrCodeGeneratorComponent } from './components/qr-code-generator/qr-code-generator.component';
import { QrCodeZoomedComponent } from './components/qr-code-generator/qr-code-zoomed/qr-code-zoomed.component';
import { NoStartOnWhiteSpaceDirective } from './directives/no-start-on-white-space.directive';
import { InputDebounceTimeDirective } from './directives/input-debounce-time.directive';
import { AreaCodeSelectorComponent } from './control-value-accessors/area-code-selector/area-code-selector.component';
import { PermissionDirective } from './directives/permission.directive';

@NgModule({
    exports: [
        CommonModule,
        FooterComponent,
        NavbarComponent,
        VerticalMenuComponent,
        HorizontalMenuComponent,
        CustomizerComponent,
        NotificationSidebarComponent,
        ToggleFullscreenDirective,
        SidebarDirective,
        TopMenuDirective,
        NgbModule,
        TranslateModule,
        DatetimePickerComponent,
        DropdownComponent,
        CheckboxComponent,
        NgSelectModule,
        NumberInputComponent,
        OnlyRealNumbersDirective,
        PasswordInputComponent,
        SwitchToggleComponent,
        TextAreaComponent,
        TextInputComponent,
        DynamicFiltersComponent,
        DynamicTableComponent,
        PaginationComponent,
        ImageComponent,
        PipeModule,
        QrCodeGeneratorComponent,
        NoStartOnWhiteSpaceDirective,
        InputDebounceTimeDirective,
        AreaCodeSelectorComponent,
        PermissionDirective
    ],
    imports: [
        RouterModule,
        CommonModule,
        NgbModule,
        TranslateModule,
        FormsModule,
        OverlayModule,
        ReactiveFormsModule ,
        PerfectScrollbarModule,
        ClickOutsideModule,
        AutocompleteModule,
        PipeModule,
        NgSelectModule,
        LightboxModule,
    ],
    declarations: [
        FooterComponent,
        NavbarComponent,
        VerticalMenuComponent,
        HorizontalMenuComponent,
        CustomizerComponent,
        NotificationSidebarComponent,
        ToggleFullscreenDirective,
        SidebarLinkDirective,
        SidebarDropdownDirective,
        SidebarAnchorToggleDirective,
        SidebarDirective,
        TopMenuLinkDirective,
        TopMenuDropdownDirective,
        TopMenuAnchorToggleDirective,
        TopMenuDirective,
        ConfirmActionDialogComponent,
        DatetimePickerComponent,
        CheckboxComponent,
        DropdownComponent,
        NumberInputComponent,
        PasswordInputComponent,
        SwitchToggleComponent,
        TextAreaComponent,
        TextInputComponent,
        PaginationComponent,
        OnlyRealNumbersDirective,
        DynamicFiltersComponent,
        DynamicFiltersDateTimeComponent,
        DynamicFiltersNumberComponent,
        DynamicFiltersSelectComponent,
        DynamicFiltersSwitchToggleComponent,
        DynamicFiltersTextComponent,
        DynamicTableComponent,
        ImageComponent,
        QrCodeGeneratorComponent,
        QrCodeZoomedComponent,
        NoStartOnWhiteSpaceDirective,
        InputDebounceTimeDirective,
        AreaCodeSelectorComponent,
        PermissionDirective,
    ]
})
export class SharedModule { }
