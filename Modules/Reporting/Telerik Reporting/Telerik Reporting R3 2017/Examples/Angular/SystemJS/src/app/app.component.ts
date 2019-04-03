import { Component } from '@angular/core';

@Component({
  selector: 'my-app',
  template: `<tr-viewer #viewer1 
    [containerStyle]="viewerContainerStyle"
    [serviceUrl]="'http://demos.telerik.com/reporting/api/reports/'"
    [reportSource]="{
        report: 'Telerik.Reporting.Examples.CSharp.ReportCatalog, CSharp.ReportLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null',
        parameters: {}
    }"
    [viewMode]="'INTERACTIVE'"
    [scaleMode]="'SPECIFIC'"
    [scale]="1.0"
    [ready]="ready"
    [viewerToolTipOpening]="viewerToolTipOpening"
    [enableAccessibility]="false">
</tr-viewer>
<button (click)="viewer1.refreshReport()">Refresh</button>
<button (click)="viewer1.commands.print.exec()">Print</button>`
})
export class AppComponent {
  title = "Report Viewer";
  viewerContainerStyle = {
    position: 'absolute',
    top: '37px',
    bottom: '5px',
    left: '5px',
    right: '5px',
    overflow: 'hidden',
    ['font-family']: 'ms sans serif'
  };

  ready() { console.log('ready'); }
  viewerToolTipOpening(e: any, args: any) { console.log('viewerToolTipOpening ' + args.toolTip.text); }
}
