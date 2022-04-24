#!/bin/bash
i=1

while [ $i -le 50 ]
do
	open -W RoombaSimulation.app --args pathingType=Random seed=$i layoutTypeArg=Square
	echo "quit"
	((i++))
done
