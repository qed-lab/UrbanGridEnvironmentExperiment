"""

/**
 * This is part of the Gaze Tracker package v2.4
 * @author C. N. Spencer
 */

Developed on Python3.8
Script that crunches the data from gaze tracking output and simplifies it by object
It uses an interactive menu to request the file to crunch and the type of analysis to be done
or can be called headlessly by using the -hl tag and providing the file name with full path
The analysis functions provided generally discard the positional data (i.e. angle, HMDpos, HMDrot)
Every participant has a list of Landmark objects - these get passed to the functions^
"""
import copy
import os
import sys
from Landmark import Landmark
import TotalLook
import MinMax
import AvgLook
import StdDev
import Var
import math

# change this for actual path to VE output
DEFAULT_FILE_PATH = "C:\\Users\\A02273854\\Unity Projects\\Testing Playground\\TutorialWorld3\\Output"


def parser(f):
	# reads the file into a list of the lines and sends to make_objs
	lines = []
	for line in f:
		if line != "time,exit,obj,angle,dist,HMDrot\n":
			lines.append((line.replace("\n", "").rsplit(",")))
	f.close()
	#for line in lines:
#		print(line)
	return make_objs(lines)


def make_objs(lines):
	# for each line in the passed in list, make_objs will find the next line of the same object/landmark and add to a
	# list of landmark objects that is returned. For exit=0 to exit=1, the time difference is added to the landmark's
	# looks list and exit=1 to exit=0 is added to the landmark's offs list. If an object has an enter code, and before
	# its next exit code, another object is listed, then the method will keep whichever object was closer to the player
	# during that time window and discard the time spent on the farther object.

	active_ln = []  # To keep track of what objects are currently looked at
	objs = []  # This holds the simplified objects to be returned
	# ln_tmp = copy.deepcopy(lines)
	prev_closest = None  # This is for checking changes in the closest object
	for i in range(len(lines) - 1):
		ln = lines[i]
		print(ln)
		# ln = [time, exit code, name, gaze ray angle from center-forward of camera, distance between object and camera, camera rotation]

		# calculate the time to add
		time = time_calc(ln[0], lines[i+1][0])  # the time between this timestamp and the next line's

		if int(ln[1]) == 0:
			active_ln.append(ln)  # mark this object as active
		elif int(ln[1]) == 1:
			print("removing...")
			active_ln.remove(ln)  # take this object off of the active list ##############  Here is the error
			print("remove successful")
		elif int(ln[1]) == -1:  # this is the game-end timestamp
			# go through all objects and add to the offs
			for obj in objs:
				if obj.name == prev_closest[2] and active_ln.__contains__(prev_closest):
					obj.offs.append(0)  # add a new off for the closest object still active (which is 0ms off)
				else:
					obj.offs[-1] += time
			print (objs)
			return objs
		print('here1')
		# find the closest of the active objects
		ln_closest = active_ln[0]
		if len(active_ln) > 1:  # need more than one object to compare
			for ln_obj in active_ln:
				if ln_closest[4] > ln_obj[4]:
					ln_closest = ln_obj  # if this object is closer, mark it as the closest object
		print("here2")
		# if the closest object has changed, make some changes to reflect the state updates
		if prev_closest != ln_closest:
			for obj in objs:
				if ln_closest[2] == obj.name:
					obj.looks.append(0)  # start a new on for new closest
				elif prev_closest[2] == obj.name and active_ln.__contains__(prev_closest):
					obj.offs.append(0)  # start a new off for the previous closest if it is still active

		# see if it is in the objects list
		done = False
		print("i maddd")
		for obj in objs:
			# if it is already in the list, add to its ons/offs list
			if ln_closest[2] == obj.name:
				obj.looks[-1] += time
				done = True
			else:
				obj.offs[-1] += time  # add to other objects' offs lists
		if not done:
			objs.append(Landmark(name=ln[2]))
		print("here3")
		# reset prev_closest
		prev_closest = ln_closest
	print("I made it here")
	return


def time_calc(time1, time2):
	# takes 2 strings of form "hr:min:sec:ms" and calculates time2 - time1
	# may not work if times around midnight when the hour changes negatively
	t1 = time1.rsplit(":")
	t2 = time2.rsplit(":")
	for i in range(len(t1)):
		try:
			t1[i] = int(t1[i])
			t2[i] = int(t2[i])
		except Exception:
			print("Error in converting timestamps")
	hours = (t2[0] - t1[0]) * 60  # minutes
	mins = (t2[1] - t1[1] + hours) * 60  # seconds
	secs = (t2[2] - t1[2] + mins) * 1000  # milliseconds
	mils = t2[3] - t1[3] + secs
	return mils


def main_menu():
	# gets a file path from the user or defaults to DEFAULT_FILE_PATH if none provided
	# not run in headless mode
	while True:
		print(f"Default file path: {DEFAULT_FILE_PATH}")
		file_path = input("File path (enter for default or \"x\" to exit main menu): ")
		if file_path.lower() == "x":  # exit
			break
		elif file_path == "":  # use default
			file_path = DEFAULT_FILE_PATH
		# try path
		try:
			print(f"Select a file from {file_path}")
			for f in os.listdir(file_path):
				print(f)
		except Exception:
			print("Bad user - gimme a real path")
		file_menu(file_path)


def file_menu(file_path):
	# open provided file and get landmarks from parser, then hand off to option menu
	while True:
		objs = []  # a list of tuples like such: (participant id, landmark objects list)
		file_in = input("File name (\"a\" or enter for all, \"x\" to exit file menu): ")
		if file_in.lower() == "x":  # exit
			break
		elif file_in.lower() == "a" or file_in == "":  # all
			for f in os.listdir(file_path):
				try:
					objs.append((f.replace("0GazeData.csv", ""),
								 parser(open(file_path + ("\\" + f if not f.__contains__("\\") else f)))))
				except IOError:
					print(f"Could not open \"{f}\" - skipping")
				except Exception:
					print(f"Could not initialize objects from \"{f}\" (probably bad format) - skipping")
		else:
			# specific file provided
			try:
				objs = [(file_in.replace("0GazeData.csv", ""),
						 parser(open(file_path + ("\\" + file_in if not file_in.__contains__("\\") else file_in))))]
			except Exception:
				print(f"Could not open \"{file_in}\"")
			break
		# only send to op_menu if the file opened correctly and objs is not empty
		if objs is not None:
			if len(objs) > 0:
				op_menu(objs)
			else:
				print("objs length is not >0")


def op_menu(objs):
	# Displays a menu of the available statistical functions; runs the chosen function
	# does not run in headless mode
	while True:
		num_ops = 6  # update this magic number when adding option(s)
		print("\nAnalysis options and descriptions:")
		print(f"1. {TotalLook.FUNCTION} - {TotalLook.DESCRIPTION}")
		print(f"2. {MinMax.FUNCTION} - {MinMax.DESCRIPTION}")
		print(f"3. {AvgLook.FUNCTION} - {AvgLook.DESCRIPTION}")
		print(f"4. {StdDev.FUNCTION} - {StdDev.DESCRIPTION}")
		print(f"5. {Var.FUNCTION} - {Var.DESCRIPTION}")
		print(f"6. All of the above")  # and this magic number, too
		print(f"{num_ops + 1}. Exit options menu")
		op_in = input("Option number: ")
		if not op_in.isnumeric():
			print("Bad user - gimme a number")
			continue
		else:
			op_in = int(op_in)
			if op_in < 1 or op_in > num_ops + 1:
				print("Bad user - gimme an option number")
				continue
		if op_in == num_ops + 1:
			break
		hl = False
		if len(objs[0][1]) >= 1:
			if type(objs[0][1][0]) is not Landmark:  # all/multiple participant's data - chosen in file menu
				for part in objs:
					print(f"\nANALYSIS FOR PARTICIPANT {part[0]}")
					if op_in == 1:
						TotalLook.total_look(part[1], hl)
					elif op_in == 2:
						MinMax.min_max(part[1], hl)
					elif op_in == 3:
						AvgLook.avg_look(part[1], hl)
					elif op_in == 4:
						StdDev.std_dev(part[1], hl)
					elif op_in == 5:
						Var.var(part[1], hl)
					elif op_in == 6:  # all options
						TotalLook.total_look(part[1], hl)
						print()
						MinMax.min_max(part[1], hl)
						print()
						AvgLook.avg_look(part[1], hl)
						print()
						StdDev.std_dev(part[1], hl)
						print()
						Var.var(part[1], hl)
			else:  # single file analysis
				print(f"\nANALYSIS FOR PARTICIPANT {objs[0][0]}")
				if op_in == 1:
					TotalLook.total_look(objs[0][1], hl)
				elif op_in == 2:
					MinMax.min_max(objs[0][1], hl)
				elif op_in == 3:
					AvgLook.avg_look(objs[0][1], hl)
				elif op_in == 4:
					StdDev.std_dev(objs[0][1], hl)
				elif op_in == 5:
					Var.var(objs[0][1], hl)
				elif op_in == 6:  # all options
					TotalLook.total_look(objs[0][1], hl)
					print()
					MinMax.min_max(objs[0][1], hl)
					print()
					AvgLook.avg_look(objs[0][1], hl)
					print()
					StdDev.std_dev(objs[0][1], hl)
					print()
					Var.var(objs[0][1], hl)


def main(hl):
	# hl is a commandline tag that stands for "headless" mode, or no interactive menus
	# if not headless, all reports will simply be printed to console and no values will be returned
	if hl:
		objs = (DEFAULT_FILE_PATH.replace("GazeData.csv", ""), parser(open(DEFAULT_FILE_PATH)))
		tot_look = TotalLook.total_look(objs[1], hl)
		minmax = MinMax.min_max(objs[1], hl)
		avg_look = AvgLook.avg_look(objs[1], hl)
		stddev = StdDev.std_dev(objs[1], hl)
		var = Var.var(objs[1], hl)

		obj_str = ""
		i = 0
		for obj in objs[1]:
			obj_str += f"{obj.name}:" + \
					   f"{obj.looks}:" + \
					   f"{obj.offs}:" + \
					   f"{tot_look[0][i][1], tot_look[0][i][2]}:".replace(')', '').replace('(', '') + \
					   f"{minmax[0][i][1], minmax[0][i][2]}:".replace(')', '').replace('(', '') + \
					   f"{avg_look[0][i][1]}:" + \
					   f"{stddev[0][i][1]}:" + \
					   f"{var[0][i][1]}\n"
			i += 1

		if stddev is None:
			out = obj_str + f"Total:{tot_look[len(tot_look) - 1]}:{minmax[len(minmax) - 1]}:{avg_look[len(avg_look) - 1]}:{None}:{None}".replace(
				'(', '').replace(')', '')
		else:
			out = obj_str + f"Total:{tot_look[len(tot_look) - 1]}:{minmax[len(minmax) - 1]}:{avg_look[len(avg_look) - 1]}:{stddev[len(stddev) - 1]}:{var[len(var) - 1]}".replace(
				'(', '').replace(')', '')
		out = out.replace('[', '').replace(']', '').replace(' ', '')
		print(out)
		return objs, tot_look, minmax, avg_look, stddev, var
	else:
		main_menu()


if __name__ == '__main__':
	hl = False
	# call from cmd/terminal/etc or python console - process args
	if len(sys.argv) >= 3:
		if sys.argv[1] == '-hl':
			DEFAULT_FILE_PATH = sys.argv[2]
			hl = True
	main(hl)
