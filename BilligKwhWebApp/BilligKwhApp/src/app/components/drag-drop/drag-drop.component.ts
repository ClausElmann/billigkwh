import { Component, OnInit } from "@angular/core";
import {
  CdkDrag,
  CdkDragDrop,
  CdkDropList,
  moveItemInArray,
  transferArrayItem
} from "@angular/cdk/drag-drop";

interface ComponentDetails {
  line: number;
  width: number;
  title: string;
}

@Component({
  selector: "app-drag-drop",
  templateUrl: "./drag-drop.component.html",
  styleUrls: ["./drag-drop.component.scss"]
})
export class DragDropComponent implements OnInit {

  public lines: ComponentDetails[] = [
    { line: 1, width: 4, title: "HPFI 4p" },
    { line: 1, width: 4, title: "GA 16 4p" },
    { line: 1, width: 2, title: "GA 10 2p" },
    { line: 2, width: 2, title: "GA 16 4p" },
    { line: 2, width: 2, title: "GA 10 2p" },
    { line: 2, width: 2, title: "GA 10 2p" },
    { line: 2, width: 4, title: "HPFI 4p" },
    { line: 3, width: 2, title: "GA 10 2p" },
    { line: 3, width: 2, title: "GA 10 2p" },
    { line: 3, width: 2, title: "GA 10 2p" },
    { line: 3, width: 4, title: "GA 13 4p" }
  ];

  public someOtherVariable: ComponentDetails[][] = [];

  ngOnInit() {
    const uniqueLines = [...new Set(this.lines.map(item => item.line))]; // [ 'A', 'B']

    function iterate(item) {
      this.someOtherVariable[item - 1] = this.lines.filter(f => f.line == item);
    }
    uniqueLines.forEach(iterate, this);
  }

  add() {
    this.lines.push({ line: 1, width: 500, title: "500" });
  }

  move() {
    transferArrayItem(
      this.someOtherVariable[0],
      this.someOtherVariable[1],
      this.someOtherVariable[0].length,
      0
    );

    //this.todo.push({ width: 500, title: "500" });
  }

  shuffle() {
    this.lines.sort(function () {
      return 0.5 - Math.random();
    });
  }

  getDivWidth(moduler: number) {
    return (moduler * 3) + "vw";
  }

  // todo = ["Get to work", "Pick up groceries", "Go home", "Fall asleep"];

  // done = ["Get up", "Brush teeth", "Take a shower", "Check e-mail", "Walk dog"];

  canDrop(item: CdkDrag, list: CdkDropList) {

    console.log(item && item.data)
    console.log(list && list.getSortedItems().length && list.getSortedItems()[0].dropContainer.data.reduce((sum, current) => sum + current.width, 0));


    // const sum = receiptItems.filter(item => item.tax === '25.00')
    //   .reduce((sum, current) => sum + current.total, 0);

    return list && list.getSortedItems().length && list.getSortedItems().length > 3;
  }


  drop(event: CdkDragDrop<string[]>) {


    // console.log((event.container.data as unknown as ComponentDetails[]).reduce((sum, current) => sum + current.width, 0))
    // console.log(event.previousIndex)

    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {

      const source = event.previousContainer.data as unknown as ComponentDetails[]
      const itemWidth = source[event.previousIndex].width;
      const targetWidth = (event.container.data as unknown as ComponentDetails[]).reduce((sum, current) => sum + current.width, 0)

      if (itemWidth + targetWidth > 12) {
        //no room on target
        return
      }

      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
    }
  }
}


/**  Copyright 2022 Google LLC. All Rights Reserved.
    Use of this source code is governed by an MIT-style license that
    can be found in the LICENSE file at https://angular.io/license */
