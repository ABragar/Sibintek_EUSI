import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = "Report Viewer";
  viewerContainerStyle = {
    position: 'relative',
    width: '1000px',
    height: '800px',
    ['font-family']: 'ms sans serif'
  };

  ready() { console.log('ready'); }
  viewerToolTipOpening(e: any, args: any) { console.log('viewerToolTipOpening ' + args.toolTip.text); }
}
