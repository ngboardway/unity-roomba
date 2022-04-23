#!/bin/bash
i=0

while [ $i -le 2 ]
do
	open -W RoombaSimulation.app --args pathingType=Random seed=$i layoutTypeArg=Square
	echo "quit"
	((i++))
done
