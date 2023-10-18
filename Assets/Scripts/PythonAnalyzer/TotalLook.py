"""

/**
 * This is part of the Gaze Tracker package v2.4
 * @author C. N. Spencer
 */

For use with Landmark objects - pass in a list of Landmarks
If hl is true, returns the sums, otherwise the report is printed to console
"""


from Landmark import Landmark

FUNCTION = "Total Looks & Durations"
DESCRIPTION = "Sums the total time spent looking and the number of glances at each landmark"


def total_look(lmks, hl):
    if len(lmks) == 0:
        print("No landmarks passed to sum totals of")
        return
    lmk_looks = []  # Each landmark's total time
    tot_time = 0
    tot_looks = 0
    for lmk in lmks:
        lmk_tot = 0
        if type(lmk) != Landmark:
            print("Not a landmark: " + str(lmk))
        else:
            for look in lmk.looks:
                lmk_tot += look
            tot_looks += len(lmk.looks)
            tot_time += lmk_tot
            if not hl:
                print(f"Total milliseconds looking at, number of looks at {lmk.name}:\t{lmk_tot}, {len(lmk.looks)}")
            else:
                lmk_looks.append((lmk.name, lmk_tot, len(lmk.looks)))
    if not hl:
        print("-------------------------------------------------------")
        print(f"Total milliseconds looking at landmarks, number of looks:\t\t{tot_time}, {tot_looks}")
    else:
        return lmk_looks, (tot_time, tot_looks)
