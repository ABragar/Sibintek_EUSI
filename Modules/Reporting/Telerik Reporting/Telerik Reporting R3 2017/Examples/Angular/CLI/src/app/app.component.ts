import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
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
