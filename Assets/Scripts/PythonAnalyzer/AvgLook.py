"""

/**
 * This is part of the Gaze Tracker package v2.4
 * @author C. N. Spencer
 */

For use with Landmark objects - pass in a list of Landmarks
If hl is true, returns the averages, otherwise the report is printed to console
"""


from Landmark import Landmark

FUNCTION = "Average Look Durations"
DESCRIPTION = "Averages the time spent looking at each landmark and all landmarks"


def avg_look(lmks, hl):
    if len(lmks) == 0:
        print("No landmarks passed to average")
        return
    lmk_looks = []  # Looks of all landmarks in one list
    tot = 0
    glances = 0
    for lmk in lmks:
        lmk_tot = 0
        if type(lmk) != Landmark:
            print("Not a landmark: " + str(lmk))
        else:
            glances += len(lmk.looks)
            for look in lmk.looks:
                lmk_tot += look
            lmk_avg = lmk_tot / len(lmk.looks)
            if not hl:
                print(f"Average milliseconds looking at {lmk.name}: {lmk_avg}")
            else:
                lmk_looks.append((lmk.name, lmk_avg))
            tot += lmk_tot
    if not hl:
        print("-------------------------------------------------------")
        print(f"Average milliseconds looking at landmarks per look:\t{tot / glances}")
        print(f"Total average milliseconds looking at landmarks:\t{tot / len(lmks)}")
    else:
        return lmk_looks, (tot / glances, tot / len(lmks))  # tuple of average per look and average per landmark
