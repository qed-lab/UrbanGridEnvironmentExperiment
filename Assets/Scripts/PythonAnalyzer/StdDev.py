"""

/**
 * This is part of the Gaze Tracker package v2.4
 * @author C. N. Spencer
 */

For use with Landmark objects - pass in a list of Landmarks
If hl is true, returns the standard deviations, otherwise the report is printed to console
"""


from Landmark import Landmark
import statistics

FUNCTION = "Standard Deviations"
DESCRIPTION = "The standard deviations of each landmark's glances and all landmarks"


def std_dev(lmks, hl):
    if len(lmks) == 0:
        print("No landmarks passed to find standard deviation of")
        return
    lmk_looks = []      # Looks of all landmarks in one list
    lmk_std_devs = []   # Each landmark's standard deviation
    for lmk in lmks:
        if type(lmk) != Landmark:
            print("Not a landmark: " + str(lmk))
        else:
            for look in lmk.looks:
                lmk_looks.append(look)
            if len(lmk.looks) <= 1:
                # print(lmk.name + " needs more data for standard deviation")
                lmk_std_devs.append((lmk.name, None))
            else:
                lmk_std_dev = statistics.stdev(lmk.looks)
                if not hl:
                    print(f"Standard deviation of {lmk.name}: {lmk_std_dev}")
                else:
                    lmk_std_devs.append((lmk.name, lmk_std_dev))
    if not hl:
        print("-------------------------------------------------------")
        print(f"Total standard deviation:\t{statistics.stdev(lmk_looks)}")
    else:
        return lmk_std_devs, statistics.stdev(lmk_looks)
